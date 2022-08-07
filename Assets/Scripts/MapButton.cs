using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : MonoBehaviour
{
    public void goToScene()
    {
        MainMenuManager.goToScene();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
