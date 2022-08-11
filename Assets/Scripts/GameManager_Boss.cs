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

    public MapGenerator_Boss mapGenerator;

    public Player myPlayer;
    public List<SwappableSpike> mySwappableSpikes;
    public layer[,] myMap;
    public layerObj[,] myMapObj;

    Vector2 movement = new Vector2(0f, 0f);

    public int n, m;
    bool isMoving = false;

    // Update is called once per frame
    void Update()
    {

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

            // swap char
            char temp = myMap[ni, nj].topLayer;
            myMap[ni, nj].topLayer = myMap[nni, nnj].topLayer;
            myMap[nni, nnj].topLayer = temp;

            var script = myMapObj[ni, nj].topLayer.GetComponent<MovableStone>();
            StartCoroutine(script.moveTo(dx, dy));

            // swap gameObject
            GameObject temp2 = myMapObj[ni, nj].topLayer;
            myMapObj[ni, nj].topLayer = myMapObj[nni, nnj].topLayer;
            myMapObj[nni, nnj].topLayer = temp2;


            playerCanMove = false;
        }

        updateSwappableSpikes();

        if (!playerCanMove)
        {
            Debug.Log("player can't move");

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
        }

        if (groundLayer == '_')
        {
            myMap[i, j].topLayer = 'N';
            myMap[i, j].groundLayer = '.';

            var script = myMapObj[i, j].groundLayer.GetComponent<PressurePlate>();
            script.Destroy();

            mapGenerator.generatePrefabs(i, j, -0.5f, 0.05f, mapGenerator.prefabs[2]);
        }

        if (newGroundLayer == 'E')
        {
            PassValue.instance.dialogueName = "Outro";
            SceneLoader.instance.loadScene(1);

            myPlayer.pass = true;
            return;
        }
        else if (newGroundLayer == '|')
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

        yield return new WaitForSeconds(0.25f);
        isMoving = false;
    }
}
