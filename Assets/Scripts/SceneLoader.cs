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
    }

    public void loadScene(int index)
    {
        if (PassValue.instance.isTutorial && 
            SceneManager.GetActiveScene().buildIndex == 2 && 
            index != 1)
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
    
    //public void loadNextScene()
    //{
    //    loadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}

    //public void loadPreviousScene()
    //{
    //    loadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}

    public IEnumerator awaitLoadScene(int index)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(1);

        SceneManager.LoadScene(index);
    }
}
