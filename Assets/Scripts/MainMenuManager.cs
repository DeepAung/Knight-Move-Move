using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenu, MapSelectMenu, AboutMenu;

    private void Start()
    {
        MainMenu.SetActive(true);
        MapSelectMenu.SetActive(false);
        AboutMenu.SetActive(false);
    }

    public static void playGame()
    {
        SceneManager.LoadScene(1);
    }

    public static void exitGame()
    {
        Debug.Log("--------------- QUIT -------------------");
        Application.Quit();
    }

    public static void goToScene()
    {
        int mapIndex = int.Parse(
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name
        );

        PassValue.instance.mapIndex = mapIndex;

        if (mapIndex == 0)
        {
            PassValue.instance.dialogueName = "Intro";
            PassValue.instance.isTutorial = true;
            SceneLoader.instance.loadScene(2);
        }
        else
        {
            SceneLoader.instance.loadScene(1);
        }
    }

}
