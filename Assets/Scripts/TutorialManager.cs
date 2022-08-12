using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameManager gameManager;

    public TMP_Text popUpText;
    public string[] popUps;
    public int popUpIndex {
        get {
            return PassValue.instance.popUpIndex;
        }

        set {
            PassValue.instance.popUpIndex = value;
        }
    }

    bool rendered, waiting;

    List<string> stage;

    // Start is called before the first frame update
    void Start()
    {
        if (!PassValue.instance.isTutorial)
        {
            Destroy(popUpText);
            Destroy(gameObject);
            return;
        }

        string path = Application.streamingAssetsPath + "/Dialogues/Tutorial.txt";
        popUps = System.IO.File.ReadAllLines(path);
        popUpIndex = PassValue.instance.popUpIndex;

        rendered = false;
        waiting = false;

        stage = new List<string>() {
            "04",
            "06",
            "16",
            "36",
            "44",
            "41",
            "01"
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!rendered)
        {
            StopAllCoroutines();
            StartCoroutine(renderPopUp());
            rendered = true;
        }

        if (popUpIndex == 0)
        {
            if (!waiting)
            {
                StartCoroutine(waitThenGoNextPopUp(2f, 1));
            }
        }
        if (popUpIndex <= 1)
        {
            if (playerIsAt(1)) goNextPopUp(1);
        }
        else if (popUpIndex == 2)
        {
            if (playerIsAt(2)) goNextPopUp(1);
        }
        else if (popUpIndex == 3)
        {
            if (playerIsAt(3)) goNextPopUp(1);
        }
        else if (popUpIndex == 4)
        {
            if (gameManager.myPlayer.moveCount <= 0 && !gameManager.myPlayer.pass)
            {
                popUpIndex++;
                //waiting = true;
                //goNextPopUp(1);
            }
            if (playerIsAt(4)) goNextPopUp(2);
        }
        else if (popUpIndex == 5)
        {
            if (playerIsAt(4)) goNextPopUp(1);
        }
        else if (popUpIndex == 6)
        {
            if (playerIsAt(5)) goNextPopUp(1);
        }
        else if (popUpIndex == 7)
        {
            if (gameManager.myPlayer.moveCount <= 0 && !gameManager.myPlayer.pass)
            {
                popUpIndex++;
                //goNextPopUp(1);
            }
            else if (playerIsAt(6)) goNextPopUp(2);
        }
        else if (popUpIndex == 8)
        {
            if (playerIsAt(6)) goNextPopUp(1);
        }
        else if (popUpIndex == 9)
        {
            // end tutorial
        }
    }

    bool playerIsAt(int index)
    {

        if (gameManager.myPlayer.position[0] == int.Parse(stage[index][0].ToString()) && 
            gameManager.myPlayer.position[1] == int.Parse(stage[index][1].ToString()))
        {
            PassValue.instance.stageIndex = index;
            return true;
        }

        return false;
    }

    void goNextPopUp(int add)
    {
        popUpIndex += add;
        rendered = false;
    }

    IEnumerator renderPopUp()
    {
        string str = popUps[popUpIndex];
        popUpText.text = "";

        for (int i = 0; i < str.Length; i++)
        {
            popUpText.text += str[i];
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    IEnumerator waitThenGoNextPopUp(float time, int add)
    {
        waiting = true;
        yield return new WaitForSeconds(time);

        popUpIndex += add;
        rendered = false;
        waiting = false;
    }
}
