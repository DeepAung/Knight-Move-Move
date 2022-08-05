using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class PassValue : MonoBehaviour
{
    public static PassValue instance;
    public int mapIndex;
    public List<int> mapList;
    public string dialogueName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    }
}
