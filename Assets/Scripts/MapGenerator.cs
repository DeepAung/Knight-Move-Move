using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class MapGenerator : MonoBehaviour
{

    [System.Serializable]
    public struct pair
    {
        public string name;
        public Tile tile;
    }

    // for debugging
    public GameObject passValuePrefab;

    public pair[] tiles;
    public Tilemap groundTilemap, topTilemap;
    public GameObject[] prefabs;
    public GameManager_Boss gameManager;
    public UIManager UI;

    string path;
    string[] mapText, firstLine;
    int n, m;
    Vector2Int offset;

    // Start is called before the first frame update
    void Awake()
    {
        // for debugging
        if (!PassValue.instance)
        {
            Instantiate(passValuePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }

        if (!PassValue.instance.isBossScene()) UI.mapNumber.text = PassValue.instance.mapNumber.ToString();
        

        loadMapFromText();

        renderMap();
    }

    void loadMapFromText()
    {
        if (PassValue.instance.isTutorial)
        {
            path = Application.streamingAssetsPath + "/Maps/Tutorial/0-" + PassValue.instance.stageIndex + ".txt";
        }
        else if (PassValue.instance.isBossScene())
        {
            Debug.Log("----------------- BOSS --------------------");
            path = Application.streamingAssetsPath + "/Maps/Boss/Boss.txt";
        }
        else
        {
            path = Application.streamingAssetsPath + "/Maps/" + PassValue.instance.mapNumber.ToString() + ".txt";
        }

        mapText = File.ReadAllLines(path);

        // assign a top line
        firstLine = mapText[0].Split();
        gameManager.n = n = int.Parse(firstLine[0]);
        gameManager.m = m = int.Parse(firstLine[1]);

        offset.x = -m / 2;
        offset.y = n / 2 - 1;
        
    }

    void renderMap()
    {
        // render all cells to background
        for (int x = -9; x <= 8; x++)
        {
            for (int y = 4; y >= -5; y--)
            {
                groundTilemap.SetTile(new Vector3Int(x, y, 0), tiles[0].tile);
            }
        }

        // set myMap length
        gameManager.myMap = new GameManager_Boss.layer[mapText.Length - 1, mapText[1].Length];
        gameManager.myPotions = new Potion[firstLine.Length - 3];

        // start render map
        for (int i = 0; i+1 < mapText.Length; i++)
        {
            for (int j = 0; j*2 < mapText[i+1].Length; j++)
            {
                char topLayer = mapText[i+1][j*2],
                     groundLayer = mapText[i+1][j*2 + 1];

                // gradually update myMap
                gameManager.myMap[i, j].topLayer = topLayer;
                gameManager.myMap[i, j].groundLayer = groundLayer;

                // render top layer
                if (topLayer == 'S') // player
                {

                    var playerObj = generatePrefabs(i, j, -0.5f, 0.25f, prefabs[0])
                        .GetComponent<Player>();

                    playerObj.position = new int[2] {i, j};
                    playerObj.moveCount = int.Parse(firstLine[2]);
                    gameManager.myPlayer = playerObj;
                    UI.myPlayer = playerObj;

                }
                else if (topLayer == 'N') // normal stone
                {
                    generatePrefabs(i, j, -0.5f, 0.05f, prefabs[2]);
                }
                else if (topLayer == 'B') // breakable stone
                {
                    var breakableStoneObj = generatePrefabs(i, j, -0.5f, 0.05f, prefabs[3])
                        .GetComponent<BreakableStone>();

                    breakableStoneObj.position[0] = i;
                    breakableStoneObj.position[1] = j;
                    gameManager.myBreakableStones.Add(breakableStoneObj);
                }
                else if (topLayer == 'M') // movable stone
                {
                    var movableStoneObj = generatePrefabs(i, j, -0.5f, 0.05f, prefabs[4])
                        .GetComponent<MovableStone>();

                    movableStoneObj.position[0] = i;
                    movableStoneObj.position[1] = j;
                    gameManager.myMovableStones.Add(movableStoneObj);
                }
                // end render


                // render ground layer
                if (groundLayer == 'X')
                {
                    generateTile(i, j, tiles[0].tile);
                }
                else if (groundLayer == '.')
                {
                    generateTile(i, j, tiles[1].tile);
                }
                else if (groundLayer == 'E') // goal  
                {
                    generateTile(i, j, tiles[1].tile);
                    generatePrefabs(i, j, -0.5f, 0.5f, prefabs[5]);
                }
                else if ('1' <= groundLayer && groundLayer <= '9') // potions
                {
                    generateTile(i, j, tiles[1].tile); // gen ground layer
                    var potionObj = generatePrefabs(i, j, -0.5f, 0.5f, prefabs[1])
                        .GetComponent<Potion>();

                    potionObj.power = int.Parse(firstLine[groundLayer - '1' + 3]);
                    gameManager.myPotions[groundLayer - '1'] = potionObj;
                }
                else if (groundLayer == '_') // pressure plate
                {
                    generateTile(i, j, tiles[1].tile); // gen ground layer
                    var pressurePlateObj = generatePrefabs(i, j, -0.5f, 0.5f, prefabs[6])
                        .GetComponent<PressurePlate>();

                    pressurePlateObj.position[0] = i;
                    pressurePlateObj.position[1] = j;
                    gameManager.myPressurePlates.Add(pressurePlateObj);
                }
                else if (groundLayer == '|') // normal spike
                {
                    var normalSpikeObj = generatePrefabs(i, j, -0.5f, 0.5f, prefabs[7])
                        .GetComponent<NormalSpike>();

                    normalSpikeObj.position[0] = i;
                    normalSpikeObj.position[1] = j;
                    gameManager.myNormalSpikes.Add(normalSpikeObj);
                }
                else if (groundLayer == '+') // swappable spike +
                {
                    var swappableSpikeObj = generatePrefabs(i, j, -0.5f, 0.5f, prefabs[8])
                        .GetComponent<SwappableSpike>();

                    swappableSpikeObj.position[0] = i;
                    swappableSpikeObj.position[1] = j;
                    swappableSpikeObj.startWith = true;
                    gameManager.mySwappableSpikes.Add(swappableSpikeObj);
                }
                else if (groundLayer == '-') // swappable spike -
                {
                    var swappableSpikeObj = generatePrefabs(i, j, -0.5f, 0.5f, prefabs[8])
                        .GetComponent<SwappableSpike>();

                    swappableSpikeObj.position[0] = i;
                    swappableSpikeObj.position[1] = j;
                    swappableSpikeObj.startWith = false;
                    gameManager.mySwappableSpikes.Add(swappableSpikeObj);
                }
                else
                {
                    generateTile(i, j, tiles[5].tile);
                }
                // end render


            }
        }
    }

    public Vector3 getPos(int i, int j, float h = 0f, float k = 0f)
    {
        return new Vector3(offset.x + j, offset.y - i, 0f);
    }
    public void generateTile(int i, int j, Tile tile)
    {
        groundTilemap.SetTile(new Vector3Int(offset.x + j, offset.y - i, 0), tile);
    }

    public GameObject generatePrefabs(int i, int j, float h, float k, GameObject obj)
    {
        return Instantiate(obj, new Vector3(offset.x+1 + j + h, offset.y - i + k, 0), Quaternion.identity);
    }

}
