using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager instance;

    public GameManager gameManager;

    public bool onHiddenRoom;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            onHiddenRoom = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
