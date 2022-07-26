using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassValue : MonoBehaviour
{
    public static PassValue instance;
    public int mapIndex;

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
    }
}
