using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenu, MapSelectMenu;

    private void Start()
    {
        MainMenu.SetActive(true);
        MapSelectMenu.SetActive(false);
    }

    public void playGame()
    {
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
        Debug.Log("--------------- QUIT -------------------");
        Application.Quit();
    }

    public void goToScene(int index)
    {
        int mapIndex = int.Parse(
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name
        );

        PassValue.instance.mapIndex = mapIndex;

        if (index == 2)
        {
            PassValue.instance.dialogueName = "Intro";
        }

        SceneLoader.instance.loadScene(index);
    }

}
