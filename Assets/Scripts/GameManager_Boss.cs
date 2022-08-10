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

    // Update is called once per frame
    void Update()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (!isMoving && movement.x != 0)
        {
            isMoving = true;

            if (!canMove(movement.x, 0f))
            {
                if (myPlayer.health <= 0)
                {
                    myPlayer.animationDestroy();
                    return;
                }

                StartCoroutine(waitForMoving());
                return;
            }

            myPlayer.enqueueMove(movement.x, 0f);
            StartCoroutine(waitForMoving());

        }
        else if (!isMoving && movement.y != 0)
        {
            isMoving = true;

            if (!canMove(0f, movement.y))
            {
                if (myPlayer.health <= 0)
                {
                    myPlayer.animationDestroy();
                    return;
                }

                StartCoroutine(waitForMoving());
                return;
            }

            myPlayer.enqueueMove(0f, movement.y);
            StartCoroutine(waitForMoving());
        }
    }

    bool canMove(float dx, float dy)
    {
        bool playerCanMove = true;

        int i = myPlayer.position[0],
              j = myPlayer.position[1],
             ni = i - (int)dy,
             nj = j + (int)dx;

        if (ni < 0 || ni >= n || nj < 0 || nj >= m) return false;

        char topLayer = myMap[i, j].topLayer,
             groundLayer = myMap[i, j].groundLayer,
             newTopLayer = myMap[ni, nj].topLayer,
             newGroundLayer = myMap[ni, nj].groundLayer;

        if (myPlayer.health <= 0) return false;
        if (newGroundLayer == 'X') return false;
        if (newTopLayer == 'N') return false;

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
                    {
                        return false;
                    }

                    // swap char
                    char temp = myMap[ni, nj].topLayer;
                    myMap[ni, nj].topLayer = myMap[nni, nnj].topLayer;
                    myMap[nni, nnj].topLayer = temp;

                    //update in myMovableStones
                    myMovableStones[it].position[0] = nni;
                    myMovableStones[it].position[1] = nnj;

                    myPlayer.moveCount--;

                    StartCoroutine(myMovableStones[it].moveObject(dx, dy));

                    playerCanMove = false;
                    break;
                }
            }
        }

        // update swappable spike
        updateSwappableSpikes();

        if (!playerCanMove)
        {
            if (groundLayer == '|')
            {
                StartCoroutine(myPlayer.animationHit());
                myPlayer.health--;
            }
            else if (groundLayer == '+' || groundLayer == '-')
            {
                for (int it = 0; it < mySwappableSpikes.Count; it++)
                {
                    if (mySwappableSpikes[it].position[0] == i && mySwappableSpikes[it].position[1] == j)
                    {
                        if (mySwappableSpikes[it].animator.GetBool("isActive"))
                        {
                            StartCoroutine(myPlayer.animationHit());
                            myPlayer.health--;
                        }

                        break;
                    }
                }
            }

            return false;
        }

        // ----- player can move ----- //

        if ('1' <= newGroundLayer && newGroundLayer <= '9' && myPotions[newGroundLayer - '1'])
        {
            myMap[ni, nj].groundLayer = '.';
            myPlayer.health++;
            myPotions[newGroundLayer - '1'].Destroy();
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

                }
            }
        }

        if (newGroundLayer == 'E')
        {
            if (PassValue.instance.mapNumber == PassValue.instance.mapList[PassValue.instance.mapList.Count - 2])
            {
                SceneLoader.instance.loadScene(3); // Boss
            }
            else
            {
                if (PassValue.instance.isTutorial)
                {
                    PassValue.instance.mapNumber = PassValue.instance.mapList[1];
                    PassValue.instance.isTutorial = false;
                    PassValue.instance.stageIndex = 0;
                    PassValue.instance.popUpIndex = 0;
                }
                else
                {
                    PassValue.instance.mapNumber = PassValue.instance.mapList[
                        PassValue.instance.mapList.IndexOf(PassValue.instance.mapNumber) + 1
                    ];
                }
                SceneLoader.instance.loadScene(2); // GamePlay
            }

            myPlayer.pass = true;
            return true;
        }
        else if (newGroundLayer == '|')
        {
            StartCoroutine(myPlayer.animationHit());
            myPlayer.health--;
        }
        else if (newGroundLayer == '+' || newGroundLayer == '-')
        {
            for (int it = 0; it < mySwappableSpikes.Count; it++)
            {
                if (mySwappableSpikes[it].position[0] == ni && mySwappableSpikes[it].position[1] == nj)
                {
                    if (mySwappableSpikes[it].animator.GetBool("isActive"))
                    {
                        StartCoroutine(myPlayer.animationHit());
                        myPlayer.health--;
                    }

                    break;
                }
            }
        }

        return playerCanMove;
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

    IEnumerator waitForMoving()
    {
        yield return new WaitForSeconds(0.25f);
        isMoving = false;
    }
}