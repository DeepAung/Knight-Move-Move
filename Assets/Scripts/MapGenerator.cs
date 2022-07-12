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

    public Player myPlayer;
    public pair[] tiles;
    public Tilemap groundTilemap, topTilemap;

    string path;
    string[] mapText;
    int n, m, cnt;
    int[] potion;

    // Start is called before the first frame update
    void Start()
    {

        loadMapFromText();

        assignValueToPlayer();

        myPlayer.moveCount = cnt;
        

    }

    void loadMapFromText()
    {
        path = Application.dataPath + "/Map/" + "1" + ".txt";
        mapText = File.ReadAllLines(path);
    }

    void assignValueToPlayer()
    {
        // assign a top line
        string[] firstLine = mapText[0].Split();
        n = int.Parse(firstLine[0]);
        m = int.Parse(firstLine[1]);
        myPlayer.moveCount = int.Parse(firstLine[2]);
        potion = new int[firstLine.Length - 3];
        for (int i = 0; i < firstLine.Length - 3; i++)
        {
            potion[i] = int.Parse(firstLine[i + 3]);
        }

        // assign the following lines
        for (int i = 1; i < mapText.Length; i++)
        {
            for (int j = 0; j < mapText[i].Length; j += 2)
            {
                char topLayer = mapText[i][j],
                     groundLayer = mapText[i][j+1];

                if (topLayer == 'P') myPlayer.pos = new Vector2Int(i, j);

                // TODO:
            }
        }

    }

}
