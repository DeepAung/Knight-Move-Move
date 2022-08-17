using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    [System.Serializable]
    public struct layer
    {
        public char topLayer;
        public char groundLayer;
    }

    public MapGenerator mapGenerator;

    public Player myPlayer;
    public Potion[] myPotions;
    public List<BreakableStone> myBreakableStones;
    public List<MovableStone> myMovableStones;
    public List<PressurePlate> myPressurePlates;
    public List<NormalSpike> myNormalSpikes;
    public List<SwappableSpike> mySwappableSpikes;
    public layer[,] myMap;

    Vector2 movement = new Vector2(0f, 0f);

    public int n, m;
    bool isMoving = false;

    private void Start()
    {
        MapParent.loadMaps();

        if (gameObject.name == "HiddenGameManager")
        {

            if (!PassValue.instance.onHiddenRoom)
            {
                Destroy(gameObject);
                return;
            }

            Camera.main.transform.position = new Vector3(0, 10, Camera.main.transform.position.z);

            string path = Application.streamingAssetsPath + "/Maps/HiddenRoom/HiddenRoom.txt";
            string[] result = System.IO.File.ReadAllLines(path);

            string[] firstLine = result[0].Split();
            n = int.Parse(firstLine[0]);
            m = int.Parse(firstLine[1]);

            myPlayer.position = new int[] { 3, 8 };
            myPlayer.moveCount = int.MaxValue;

            myMap = new layer[result.Length - 1, result[1].Length];

            for (int i = 0; i+1 < result.Length; i++)
            {
                for (int j = 0; j*2 < result[i+1].Length; j++)
                {
                    char topLayer = result[i + 1][j * 2],
                         groundLayer = result[i + 1][j * 2 + 1];

                    myMap[i, j].topLayer = topLayer;
                    myMap[i, j].groundLayer = groundLayer;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "GameManager")
        {
            if (PassValue.instance.onHiddenRoom) return;
        }

        if (gameObject.name == "HiddenGameManager")
        {
            if (PassValue.instance.popUpIndex != -1) return;
        }

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

            if (!myPlayer.pass && myPlayer.moveCount <= 0)
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

        char topLayer       = myMap[i, j].topLayer,
             groundLayer    = myMap[i, j].groundLayer,
             newTopLayer    = myMap[ni, nj].topLayer,
             newGroundLayer = myMap[ni, nj].groundLayer;

        if (myPlayer.moveCount <= 0) return;
        if (newGroundLayer == 'X') return;
        if (newTopLayer == 'N') return;

        Debug.Log($"cur: {i},{j},{topLayer},{groundLayer} | new: {ni},{nj},{newTopLayer},{newGroundLayer}");


        if (newTopLayer == 'B')
        {
            for (int it = 0; it < myBreakableStones.Count; it++)
            {
                if (myBreakableStones[it].position[0] == ni && myBreakableStones[it].position[1] == nj)
                {
                    myMap[ni, nj].topLayer = ' ';
                    myBreakableStones[it].Destroy();
                    myPlayer.moveCount--;

                    AudioManager.instance.play("WoodBroken");

                    playerCanMove = false;
                    break;
                }
            }
        }
        else if (newTopLayer == 'M')
        {
            for (int it = 0; it < myMovableStones.Count; it++)
            {
                if (myMovableStones[it].position[0] == ni && myMovableStones[it].position[1] == nj)
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

                    //update in myMovableStones
                    myMovableStones[it].position[0] = nni;
                    myMovableStones[it].position[1] = nnj;

                    myPlayer.moveCount--;

                    StartCoroutine(myMovableStones[it].moveTo(dx, dy));

                    AudioManager.instance.play("StoneMove");

                    playerCanMove = false;
                    break;
                }
            }
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
                for (int it = 0; it < mySwappableSpikes.Count; it++)
                {
                    if (mySwappableSpikes[it].position[0] == i && mySwappableSpikes[it].position[1] == j)
                    {
                        if (mySwappableSpikes[it].animator.GetBool("isActive"))
                        {
                            myPlayer.getDamage();
                        }

                        break;
                    }
                }
            }

            return;
        }

        // ----------------- player can move ----------------- //

        if (gameObject.name == "HiddenGameManager")
        {
            if (myPlayer.position[0] - dy == 7 && 
                myPlayer.position[1] + dx == 8)
            {
                PassValue.instance.onHiddenRoom = false;
                SceneLoader.instance.loadScene(2, 0.2f);
                Destroy(myPlayer, 0.5f);
            }
        }

        myPlayer.moveCount--;
        myPlayer.enqueueMove(dx, dy);

        if ('1' <= newGroundLayer && newGroundLayer <= '9' && myPotions[newGroundLayer - '1'])
        {
            myMap[ni, nj].groundLayer = '.';
            myPlayer.moveCount += myPotions[newGroundLayer - '1'].power;
            myPotions[newGroundLayer - '1'].Destroy();

            AudioManager.instance.play("GetPotion");
        }

        if (groundLayer == '_')
        {
            for (int it = 0; it < myPressurePlates.Count; it++)
            {
                if (myPressurePlates[it].position[0] == i && myPressurePlates[it].position[1] == j)
                {
                    myMap[i, j].topLayer = 'N';
                    myMap[i, j].groundLayer = '.';

                    myPressurePlates[it].Destroy();
                    mapGenerator.generatePrefabs(i, j, -0.5f, 0.05f, mapGenerator.prefabs[2]);

                    AudioManager.instance.play("StoneMove");

                }
            }
        }

        if (newGroundLayer == 'E')
        {

            if (PassValue.instance.mapNumber == PassValue.instance.mapList[PassValue.instance.mapList.Count - 2])
            {
                PassValue.instance.dialogueName = "Warning";
                SceneLoader.instance.loadScene(1);
            }
            //if (PassValue.instance.mapNumber == PassValue.instance.mapList[PassValue.instance.mapList.Count - 1])
            //{
            //    PassValue.instance.dialogueName = "Outro";
            //    SceneLoader.instance.loadScene(1);
            //}
            else
            {
                PassValue.instance.mapNumber = PassValue.instance.mapList[
                    PassValue.instance.mapList.IndexOf(PassValue.instance.mapNumber) + 1
                ];

                SceneLoader.instance.loadScene(2); // GamePlay
            }

            AudioManager.instance.play("Teleport");

            myPlayer.animator.SetTrigger("Warp");

            myPlayer.pass = true;
            return;
        }
        else if (newGroundLayer == '|')
        {
            myPlayer.getDamage();
        }
        else if (newGroundLayer == '+' || newGroundLayer == '-')
        {
            for (int it = 0; it < mySwappableSpikes.Count; it++)
            {
                if (mySwappableSpikes[it].position[0] == ni && mySwappableSpikes[it].position[1] == nj)
                {
                    if (mySwappableSpikes[it].animator.GetBool("isActive"))
                    {
                        myPlayer.getDamage();
                    }

                    break;
                }
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
}
