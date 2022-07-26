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
    }

    // Update is called once per frame
    void Update()
    {
        moveCount.text = myPlayer.moveCount.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // restart scene
            SceneManager.LoadScene(1);
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
        SceneManager.LoadScene(0);
    }


}
