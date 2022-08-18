using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text showText, helpText;
    public Image showImage;

    public TMP_FontAsset beforeBossFont, warningFont;

    Animator showTextAnim, showImageAnim;

    string[] textFile;
    Sprite imageSprite;

    bool finished = false;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        //hide image
        showImage.enabled = false;
        helpText.enabled = false;

        showTextAnim = showText.GetComponent<Animator>();
        showImageAnim = showImage.GetComponent<Animator>();

        var result = Resources.Load<TextAsset>("Dialogues/" + PassValue.instance.dialogueName);
        textFile = result.text.Split('\n');


        if (PassValue.instance.dialogueName == "BeforeBoss")
        {
            showText.font = beforeBossFont;

            showImage.enabled = true;

            imageSprite = Resources.Load<Sprite>("DialoguesImage/BeforeBoss");
            showImage.sprite = imageSprite;
            showImageAnim.SetTrigger("Start");
        }
        else if (PassValue.instance.dialogueName == "Warning")
        {
            showText.font = warningFont;

            showText.color = Color.red;
        }
        StartCoroutine(showDialogues());

    }

    // Update is called once per frame
    void Update()
    {

        if (finished && 
            (Input.GetKeyDown(KeyCode.Space) ||
             Input.GetKeyDown(KeyCode.Return) ||
             Input.GetKeyDown(KeyCode.KeypadEnter)))
        {

            AudioManager.instance.play("ButtonClick");
            index++;

            if (index == textFile.Length)
            {
                finished = false;
                if (PassValue.instance.dialogueName == "Warning")
                {
                    PassValue.instance.dialogueName = "BeforeBoss";
                    SceneLoader.instance.loadScene(1);
                }
                else if (PassValue.instance.dialogueName == "BeforeBoss")
                {
                    SceneLoader.instance.loadScene(3);
                }
                else if (PassValue.instance.dialogueName == "Outro")
                {
                    SceneLoader.instance.loadScene(0);
                }
            }
            else
            {
                if (PassValue.instance.dialogueName == "BeforeBoss")
                {
                    if (index == 6 || index == 9 || index == 12 || index == 13)
                        showText.color = Color.red;
                    else
                        showText.color = Color.white;
                }
                StartCoroutine(showDialogues());
            }

        }
    }

    IEnumerator showDialogues()
    {

        showText.text = textFile[index];
        showTextAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        helpText.enabled = true;
        finished = true;
    }
}
