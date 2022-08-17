using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenu, MapSelectMenu, AboutMenu;

    private void Awake()
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

    public static void goToGithub()
    {
        Application.OpenURL("https://github.com/DeepAung/Knight-Move-Move");
    }

    public void goToIntro()
    {
        MapParent.loadMaps();

        PassValue.instance.mapNumber = 0;
        SceneLoader.instance.loadScene(2);
    }

    public static void goToScene()
    {
        int buttonName = int.Parse(
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name
        );

        PassValue.instance.mapNumber = buttonName;

        if (buttonName == int.MaxValue)
        {
            SceneLoader.instance.loadScene(3); // Boss
        }
        else
        {
            SceneLoader.instance.loadScene(2); // GamePlay
        }
    }

}
