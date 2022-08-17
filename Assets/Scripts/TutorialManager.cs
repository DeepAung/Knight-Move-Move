using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameManager gameManager;

    public Image bg, blackGround, showImage;
    readonly Color show = new Color(1f, 1f, 1f, 0.5f),
                   hide = new Color(1f, 1f, 1f, 0f);

    public TMP_Text popUpText;
    public string[] popUps, popUpsIntro;
    public int popUpIndex {
        get {
            return PassValue.instance.popUpIndex;
        }

        set {
            PassValue.instance.popUpIndex = value;
        }
    }
    public int popUpIndexAfterIntro = 0;

    bool rendered, waiting;

    List<string> stage;

    // Start is called before the first frame update
    void Start()
    {
        bg.color = show;

        if (!PassValue.instance.isTutorial)
        {
            blackGround.enabled = false;
            showImage.enabled = false;
            bg.color = hide;
            Destroy(popUpText);
            Destroy(gameObject);
            return;
        }

        popUpsIntro = Resources.Load<TextAsset>("Dialogues/Intro").text.Split('\n');

        showImage.sprite = Resources.Load<Sprite>("DialoguesImage/Intro");

        popUpIndex = PassValue.instance.popUpIndex;
        if (!PassValue.instance.onHiddenRoom || popUpIndex != 0)
        {
            Debug.Log("test");
            blackGround.enabled = false;
            showImage.enabled = false;
        }

        popUps = Resources.Load<TextAsset>("Dialogues/Tutorial").text.Split('\n');

        rendered = false;
        waiting = false;

        stage = new List<string>() {
            "04",
            "24",
            "26",
            "36",
            "56",
            "64",
            "61",
            "21"
        };
    }

    // Update is called once per frame
    void Update()
    {
        // for AfterIntro (when wizard talks to player in hidden room)
        if (PassValue.instance.onHiddenRoom)
        {
            if (popUpIndex == -1) return;

            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Return) || 
                Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                goNextPopUp(1);
                if (popUpIndex == 1)
                {
                    blackGround.GetComponent<Animator>().SetTrigger("Fadeout");
                }
                else if (popUpIndex == popUpsIntro.Length)
                {
                    popUpIndex = -1;
                    return;
                }
            }

            if (!rendered)
            {
                StopAllCoroutines();
                StartCoroutine(renderPopUpAfterIntro());
                rendered = true;
            }

            return;
        }

        if (popUpIndex == -1)
        {
            if (playerIsAt(1)) goNextPopUp(1);
            return;
        }

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
                bg.color = show;

                StartCoroutine(waitThenGoNextPopUp(2f, 1));
            }
        }
        if (popUpIndex <= 1)
        {
            if (playerIsAt(2)) goNextPopUp(1);
        }
        else if (popUpIndex == 2)
        {
            if (playerIsAt(3)) goNextPopUp(1);
        }
        else if (popUpIndex == 3)
        {
            if (playerIsAt(4)) goNextPopUp(1);
        }
        else if (popUpIndex == 4)
        {
            if (gameManager.myPlayer.moveCount <= 0 && !gameManager.myPlayer.pass)
            {
                popUpIndex++;
            }
            if (playerIsAt(5)) goNextPopUp(2);
        }
        else if (popUpIndex == 5)
        {
            if (playerIsAt(5)) goNextPopUp(1);
        }
        else if (popUpIndex == 6)
        {
            if (playerIsAt(6)) goNextPopUp(1);
        }
        else if (popUpIndex == 7)
        {
            if (gameManager.myPlayer.moveCount <= 0 && !gameManager.myPlayer.pass)
            {
                popUpIndex++;
            }
            else if (playerIsAt(7))
            {
                goNextPopUp(2);
            }
        }
        else if (popUpIndex == 8)
        {
            if (playerIsAt(7))
            {
                goNextPopUp(1);
            }
        }
        else if (popUpIndex == 9)
        {

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
        AudioManager.instance.play("ButtonClick");
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
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator renderPopUpAfterIntro()
    {
        string str = popUpsIntro[popUpIndex];
        popUpText.text = "";

        for (int i = 0; i < str.Length; i++)
        {
            popUpText.text += str[i];
            yield return new WaitForSeconds(0.01f);
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
