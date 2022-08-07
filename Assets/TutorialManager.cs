using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameManager gameManager;

    public TMP_Text popUpText;
    public string[] popUps;
    public int popUpIndex
    {
        get {
            return PassValue.instance.popUpIndex;
        }

        set {
            PassValue.instance.popUpIndex = value;
        }
    }

    bool isRendering;

    // Start is called before the first frame update
    void Start()
    {
        if (!PassValue.instance.isTutorial)
        {
            Destroy(gameObject);
            return;
        }

        string path = Application.streamingAssetsPath + "/Dialogues/Tutorial.txt";
        popUps = System.IO.File.ReadAllLines(path);
        popUpIndex = PassValue.instance.popUpIndex;
        isRendering = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (popUpIndex == -1)
        {
            StartCoroutine(renderNextPopUp());
        }
        if (popUpIndex == 0)
        {
            StartCoroutine(renderNextPopUp());
        }
        else if (popUpIndex == 1)
        {
            if (playerIsAt(0, 6)) StartCoroutine(renderNextPopUp());
        }
        else if (popUpIndex == 2)
        {
            if (playerIsAt(1, 6)) StartCoroutine(renderNextPopUp());
        }
        else if (popUpIndex == 3)
        {
            if (playerIsAt(3, 6)) StartCoroutine(renderNextPopUp());
        }
        else if (popUpIndex == 4)
        {
            //if (player died) popUpIndex++;
            if (playerIsAt(4, 4)) StartCoroutine(renderNextPopUp(2));
        }
        else if (popUpIndex == 5)
        {
            if (playerIsAt(4, 4)) StartCoroutine(renderNextPopUp());
        }
        else if (popUpIndex == 6)
        {
            if (playerIsAt(4, 1)) StartCoroutine(renderNextPopUp());
        }
        else if (popUpIndex == 7)
        {
            //if (player died) popUpIndex++;
            if (playerIsAt(0, 1)) StartCoroutine(renderNextPopUp(2));
        }
        else if (popUpIndex == 8)
        {
            if (playerIsAt(0, 1)) StartCoroutine(renderNextPopUp());
        }
        else if (popUpIndex == 9)
        {
            if (!isRendering)
            {
                isRendering = true;
                PassValue.instance.playerLastPos = new int[] { 0, 4 };
            }
        }
    }

    bool playerIsAt(int i, int j)
    {
        PassValue.instance.playerLastPos = new int[] { i, j };
        return (gameManager.myPlayer.position[0] == i && gameManager.myPlayer.position[1] == j);
    }

    IEnumerator renderNextPopUp(int add = 1)
    {
        if (isRendering) yield break;

        // save last player position
        

        popUpIndex += add;
        isRendering = true;

        string str = popUps[popUpIndex];
        popUpText.text = "";

        for (int i = 0; i < str.Length; i++)
        {
            popUpText.text += str[i];
            //yield return new WaitForSecondsRealtime(0.01f);
        }

        isRendering = false;

    }
}
