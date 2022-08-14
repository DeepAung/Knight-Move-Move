using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public TMP_Text moveCount, mapNumber;
    public GameObject PauseMenu, ShowStats;
    public Player myPlayer;

    bool isBossSceneStore;

    bool toggle = false;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowStats.SetActive(!toggle);
        PauseMenu.SetActive(toggle);

        isBossSceneStore = PassValue.instance.isBossScene;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBossSceneStore) moveCount.text = myPlayer.moveCount.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePauseMenu();
            AudioManager.instance.play("ButtonSelect");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            // restart scene
            SceneLoader.instance.restartScene();
            AudioManager.instance.play("ButtonSelect");
        }
    }

    public void togglePauseMenu()
    {
        toggle = !toggle;

        if (toggle) Time.timeScale = 0;
        else Time.timeScale = 1;

        ShowStats.SetActive(!toggle);
        PauseMenu.SetActive(toggle);
    }

    public void goToMainMenu()
    {
        Time.timeScale = 1;
        SceneLoader.instance.loadScene(0); // MainMenu
    }


}
