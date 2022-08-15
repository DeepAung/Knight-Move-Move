using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public Boss boss;
    public GameManager_Boss gameManager;
    public MapGenerator_Boss mapGenerator;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss")
            .GetComponent<Boss>();

        gameManager = GameObject.FindGameObjectWithTag("GameManager_Boss")
            .GetComponent<GameManager_Boss>();

        mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator_Boss")
            .GetComponent<MapGenerator_Boss>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collector collide");
        if (collision.gameObject.CompareTag("MovableStone"))
        {
            boss.health--;
            if (boss.health == 0)
            {
                boss.animator.SetTrigger("Dead");
                PassValue.instance.dialogueName = "Outro";
                SceneLoader.instance.loadScene(1, 3f);
            }
            else
            {
                boss.animator.SetTrigger("Hit");
            }


            int[] pos = collision.gameObject.GetComponent<MovableStone>().position;
            int i = pos[0], j = pos[1];

            gameManager.myMap[i, j].topLayer = ' ';
            gameManager.myMapObj[i, j].topLayer = null;
            Destroy(collision.gameObject);


            i = Random.Range(0, gameManager.n);
            j = Random.Range(0, gameManager.m);
            mapGenerator.generatePrefabs(i, j, -0.5f, 0.5f, mapGenerator.prefabs[9]);
            Destroy(gameObject);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("collector collide");
    //    if (collision.gameObject.CompareTag("MovableStone"))
    //    {
    //        boss.health--;
    //        if (boss.health == 0)
    //        {
    //            boss.animator.SetTrigger("Dead");
    //            PassValue.instance.dialogueName = "Outro";
    //            SceneLoader.instance.loadScene(1, 3f);
    //        }
    //        else
    //        {
    //            boss.animator.SetTrigger("Hit");
    //        }


    //        int[] pos = gameObject.GetComponent<MovableStone>().position;
    //        int i = pos[0], j = pos[1];

    //        gameManager.myMap[i, j].topLayer = ' ';
    //        gameManager.myMapObj[i, j].topLayer = null;
    //        Destroy(collision.gameObject);


    //        i = Random.Range(0, gameManager.n);
    //        j = Random.Range(0, gameManager.m);
    //        mapGenerator.generatePrefabs(i, j, -0.5f, 0.5f, mapGenerator.prefabs[9]);
    //        Destroy(gameObject);
    //    }
    //}
}
