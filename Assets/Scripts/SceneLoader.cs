using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public static SceneLoader instance;
    public Animator animator;

    private void Awake()
    {
        instance = this;

        int currIndex = SceneManager.GetActiveScene().buildIndex;
        if (currIndex == 3)
            PassValue.instance.isBossScene = true;
        else
            PassValue.instance.isBossScene = false;

        if (currIndex == 2 && PassValue.instance.mapNumber == 0)
            PassValue.instance.isTutorial = true;
        else
        {
            PassValue.instance.isTutorial = false;
            PassValue.instance.stageIndex = 0;
            PassValue.instance.popUpIndex = 0;
        }
    }

    public void loadScene(int index)
    {

        if (index == 3)
            PassValue.instance.isBossScene = true;
        else
            PassValue.instance.isBossScene = false;

        if (index == 2 && PassValue.instance.mapNumber == 0)
            PassValue.instance.isTutorial = true;
        else
        {
            PassValue.instance.isTutorial = false;
            PassValue.instance.stageIndex = 0;
            PassValue.instance.popUpIndex = 0;
        }

        StartCoroutine( awaitLoadScene(index) );
    }

    public void restartScene()
    {
        loadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator awaitLoadScene(int index)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(index);
    }
}
