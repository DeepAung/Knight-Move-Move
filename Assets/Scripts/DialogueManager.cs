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

    Animator showTextAnim, showImageAnim;

    string[] textFile;
    List<Sprite> imageSpriteList;

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        showTextAnim = showText.GetComponent<Animator>();
        showImageAnim = showImage.GetComponent<Animator>();

        string path = Application.streamingAssetsPath + "/Dialogues/" + PassValue.instance.dialogueName + ".txt";
        textFile = File.ReadAllLines(path);

        showText.text = "";

        imageSpriteList = new List<Sprite>(
            Resources.LoadAll<Sprite>("Image/" + PassValue.instance.dialogueName)
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (index == imageSpriteList.Count) return;


        showText.text = textFile[index];
        showImage.sprite = imageSpriteList[index];


        if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            index++;
            AudioManager.instance.play("ButtonClick");

            if (index == imageSpriteList.Count)
            {
                if (PassValue.instance.dialogueName == "Intro")
                    SceneLoader.instance.loadScene(2);
                else if (PassValue.instance.dialogueName == "Outro")
                    SceneLoader.instance.loadScene(0);
            }
            else
            {
                showTextAnim.SetTrigger("Start");
                showImageAnim.SetTrigger("Start");
            }
        }
    }
}
