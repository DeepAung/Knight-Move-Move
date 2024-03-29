using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager_Boss : MonoBehaviour
{

    [System.Serializable]
    public struct layer
    {
        public char topLayer;
        public char groundLayer;
    }

    [System.Serializable]
    public struct layerObj
    {
        public GameObject topLayer;
        public GameObject groundLayer;
    }

    [System.Serializable]
    public struct Array2D
    {
        public layer[] arr;
    }


    public MapGenerator_Boss mapGenerator;

    public Player myPlayer;
    public List<SwappableSpike> mySwappableSpikes;
    public layer[,] myMap;
    public layerObj[,] myMapObj;

    public Array2D[] showMap;

    public Boss boss;

    Vector2 movement = new Vector2(0f, 0f);

    public int n, m;
    bool isMoving = false;

    private void Start()
    {
        int randI = Random.Range(0, n);
        int randJ = Random.Range(0, m);
        mapGenerator.generatePrefabs(randI, randJ, -0.5f, 0.5f, mapGenerator.prefabs[9]);

        showMap = new Array2D[n];
        for (int i = 0; i < n; i++)
        {
            showMap[i].arr = new layer[m];
            for (int j = 0; j < m; j++)
            {
                showMap[i].arr[j].topLayer = myMap[i, j].topLayer;
                showMap[i].arr[j].groundLayer = myMap[i, j].groundLayer;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ----- for debugging ----- //
        //for (int i = 0; i < n; i++)
        //{
        //    for (int j = 0; j < m; j++)
        //    {
        //        showMap[i].arr[j].topLayer = myMap[i, j].topLayer;
        //        showMap[i].arr[j].groundLayer = myMap[i, j].groundLayer;
        //    }
        //}

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (!isMoving)
        {
            isMoving = true;

            if (movement.x != 0) tryToMove(movement.x, 0f);
            else if (movement.y != 0) tryToMove(0f, movement.y);
            else
            {
                isMoving = false;
                return;
            }

            if (myPlayer.health <= 0)
            {
                myPlayer.animationDestroy();
                return;
            }

            StartCoroutine(waitForNextMoving());
        }
    }

    void tryToMove(float dx, float dy)
    {
        bool playerCanMove = true;

        int i = myPlayer.position[0],
              j = myPlayer.position[1],
             ni = i - (int)dy,
             nj = j + (int)dx;

        AudioManager.instance.play("Walk");

        if (ni < 0 || ni >= n || nj < 0 || nj >= m) return;

        char topLayer = myMap[i, j].topLayer,
             groundLayer = myMap[i, j].groundLayer,
             newTopLayer = myMap[ni, nj].topLayer,
             newGroundLayer = myMap[ni, nj].groundLayer;

        if (myPlayer.health <= 0) return;
        if (newGroundLayer == 'X') return;
        if (newTopLayer == 'N') return;

        Debug.Log($"cur: {i},{j},{topLayer},{groundLayer} | new: {ni},{nj},{newTopLayer},{newGroundLayer}");


        if (newTopLayer == 'B')
        {
            myMap[ni, nj].topLayer = ' ';
            var script = myMapObj[ni, nj].topLayer.GetComponent<BreakableStone>();
            script.Destroy();

            AudioManager.instance.play("WoodBroken");

            playerCanMove = false;
        }
        else if (newTopLayer == 'M')
        {

            int nni = ni - (int)dy, nnj = nj + (int)dx;
            if (nni < 0 || nni >= n || nnj < 0 || nnj >= m ||
                myMap[nni, nnj].topLayer != ' ' ||
                myMap[nni, nnj].groundLayer == 'X')
            { // if cannot move.
                return;
            }

            var script = myMapObj[ni, nj].topLayer.GetComponent<MovableStone>();

            // swap char
            char temp = myMap[ni, nj].topLayer;
            myMap[ni, nj].topLayer = myMap[nni, nnj].topLayer;
            myMap[nni, nnj].topLayer = temp;

            // swap gameObject
            GameObject temp2 = myMapObj[ni, nj].topLayer;
            myMapObj[ni, nj].topLayer = myMapObj[nni, nnj].topLayer;
            myMapObj[nni, nnj].topLayer = temp2;

            script.position = new int[] { nni, nnj };

            StartCoroutine(script.moveTo(dx, dy));

            AudioManager.instance.play("StoneMove");

            playerCanMove = false;

        }

        updateSwappableSpikes();

        if (!playerCanMove)
        {

            if (groundLayer == '|')
            {
                myPlayer.getDamage();
            }
            else if (groundLayer == '+' || groundLayer == '-')
            {
                var script = myMapObj[i, j].groundLayer.GetComponent<SwappableSpike>();
                if (script.animator.GetBool("isActive"))
                {
                    myPlayer.getDamage();
                }
            }

            return;
        }

        // ----------------- player can move ----------------- //

        myPlayer.enqueueMove(dx, dy);

        if ('1' <= newGroundLayer && newGroundLayer <= '9')
        {
            myMap[ni, nj].groundLayer = '.';

            var script = myMapObj[ni, nj].groundLayer.GetComponent<Potion>();
            myPlayer.health += script.power;
            script.Destroy();

            AudioManager.instance.play("GetPotion");
        }

        if (groundLayer == '_')
        {
            myMap[i, j].topLayer = 'N';
            myMap[i, j].groundLayer = '.';

            var script = myMapObj[i, j].groundLayer.GetComponent<PressurePlate>();
            script.Destroy();

            mapGenerator.generatePrefabs(i, j, -0.5f, 0.05f, mapGenerator.prefabs[2]);

            AudioManager.instance.play("StoneMove");
        }

        /*if (newGroundLayer == 'E')
        {
            PassValue.instance.dialogueName = "Outro";
            SceneLoader.instance.loadScene(1);

            AudioManager.instance.play("Teleport");

            myPlayer.animator.SetTrigger("Warp");

            myPlayer.pass = true;
            return;
        }
        else */if (newGroundLayer == '|')
        {
            myPlayer.getDamage();
        }
        else if (newGroundLayer == '+' || newGroundLayer == '-')
        {
            var script = myMapObj[ni, nj].groundLayer.GetComponent<SwappableSpike>();
            if (script.animator.GetBool("isActive"))
            {
                myPlayer.getDamage();
            }
        }
    }

    void updateSwappableSpikes()
    {
        for (int i = 0; i < mySwappableSpikes.Count; i++)
        {

            mySwappableSpikes[i].animator.SetBool(
                "isActive",
                !mySwappableSpikes[i].animator.GetBool("isActive")
            );
        }
    }

    IEnumerator waitForNextMoving()
    {

        yield return new WaitForSeconds(0.2f);
        isMoving = false;
    }

    public void canSpikeDealDamage(int i, int j)
    {
        if (myPlayer.position[0] == i && myPlayer.position[1] == j)
        {
            myPlayer.getDamage();
            if (myPlayer.health <= 0)
            {
                myPlayer.animationDestroy();
            }
        }
    }

    //public void genNewTeleport(int pi, int pj)
    //{
    //    Destroy(myMapObj[pi, pj].groundLayer);
    //    myMap[pi, pj].groundLayer = '.';
    //    myMapObj[pi, pj].groundLayer = null;

    //    int i, j;
    //    do
    //    {
    //        i = Random.Range(0, n);
    //        j = Random.Range(0, m);
    //    } while (myMap[i, j].topLayer != ' ' || 
    //             myMap[i, j].groundLayer != '.');

    //    Debug.LogWarning("gen new teleport: out of loop");
    //    GameObject obj = mapGenerator.generatePrefabs(i, j, -0.5f, 0.5f, mapGenerator.prefabs[5]);
    //    myMap[i, j].groundLayer = 'E';
    //    myMapObj[i, j].groundLayer = obj;
    //}
}
