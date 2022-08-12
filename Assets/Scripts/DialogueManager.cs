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
    //public Image[] introImages, outroImages;

    string[] textFile;
    List<Sprite> imageSpriteList;

    bool readyToLoadScene = false;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
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


        if (Input.anyKeyDown)
        {
            index++;

            if (index == imageSpriteList.Count)
            {
                if (PassValue.instance.dialogueName == "Intro")
                    SceneLoader.instance.loadScene(2);
                else if (PassValue.instance.dialogueName == "Outro")
                    SceneLoader.instance.loadScene(0);
            }
        }
    }
}
