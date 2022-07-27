using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText, helpText;
    string[] textFile;
    bool readyToLoadScene = false;

    // Start is called before the first frame update
    void Start()
    {
        string path = Application.streamingAssetsPath + "/Dialogues/" + PassValue.instance.dialogueName + ".txt";
        textFile = File.ReadAllLines(path);

        dialogueText.text = "";
        StartCoroutine( showText() );
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (readyToLoadScene)
            {
                if (PassValue.instance.dialogueName == "Intro")
                {
                    SceneLoader.instance.loadScene(1);
                }
                else if (PassValue.instance.dialogueName == "Outro")
                {
                    SceneLoader.instance.loadScene(0);
                }
            }
            else
            {
                skipDialogue();
            }
        }
    }

    IEnumerator showText()
    {
        for (int i = 0; i < textFile.Length; i++)
        {
            for (int j = 0; j < textFile[i].Length; j++)
            {
                dialogueText.text += textFile[i][j];
                yield return new WaitForSecondsRealtime(0.05f);
            }
            dialogueText.text += "\n";
            yield return new WaitForSecondsRealtime(0.2f);
        }

        readyToLoadScene = true;
        helpText.text = "Press anykey to continue.";
    }

    void skipDialogue()
    {
        StopAllCoroutines();

        dialogueText.text = "";
        for (int i = 0; i < textFile.Length; i++)
        {
            for (int j = 0; j < textFile[i].Length; j++)
            {
                dialogueText.text += textFile[i][j];
            }
            dialogueText.text += "\n";
        }

        readyToLoadScene = true;
        helpText.text = "Press anykey to continue.";
    }
}
