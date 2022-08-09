using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class PassValue : MonoBehaviour
{
    [System.Serializable]
    public struct layer
    {
        public char topLayer;
        public char groundLayer;
    }

    public static PassValue instance;
    public int mapNumber = 0;
    public List<int> mapList;
    public string dialogueName;

    // for tutorials
    public bool isTutorial;
    public int popUpIndex;
    public int stageIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            isTutorial = false;
            popUpIndex = 0;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        string path = Application.streamingAssetsPath + "/Maps/";
        var dirInfo = new DirectoryInfo(path);
        var files = dirInfo.GetFiles("*.txt");

        for (int i = 0; i < files.Length; i++)
        {
            bool canParse = int.TryParse(files[i].Name[0..^4], out int result);
            if (canParse)
            {
                mapList.Add(result);
            }
        }
        mapList.Sort();

    }
}
