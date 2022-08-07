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
        StartCoroutine( awaitLoadScene(index) );
    }

    public IEnumerator awaitLoadScene(int index)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(1);

        SceneManager.LoadScene(index);
    }
}
