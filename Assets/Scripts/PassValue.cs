using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassValue : MonoBehaviour
{
    public static PassValue instance;

    public int mapNumber = 0;
    public List<int> mapList;
    public string dialogueName;

    // for tutorials
    public bool isTutorial;
    public bool onHiddenRoom;
    public int popUpIndex;
    public int stageIndex;

    // for boss scene
    public bool isBossScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // initialize value for tutorials
            isBossScene = false;
            isTutorial = false;
            popUpIndex = 0;
            stageIndex = 0;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
