using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    int health = 10;
    public GameManager_Boss gameManager;
    public MapGenerator mapGenerator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attack(float time)
    {
        Debug.Log("attack()");

        int i, j;

        do
        {
            i = Random.Range(0, gameManager.n);
            j = Random.Range(0, gameManager.m);
        } while (gameManager.myMap[i, j].topLayer != ' ');
        gameManager.myMap[i, j].topLayer = '!'; // pending

        Transform obj = mapGenerator.generatePrefabs(-2, j, -0.5f, 0.05f, mapGenerator.prefabs[4])
            .transform;

        StartCoroutine(objFallDown(obj, i, j, time));
    }

    IEnumerator objFallDown(Transform obj, int i, int j, float time)
    {
        Vector2 target = mapGenerator.getPos(i, j);
        float speed = 50 / time;

        while (obj.position.y < target.y)
        {
            Vector2.MoveTowards(obj.position, target, speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        obj.position = target;
        yield return new WaitForFixedUpdate();

        gameManager.myMap[i, j].topLayer = 'M';
    }
}
