using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public HealthBar healthBar;
    public GameManager_Boss gameManager;
    public MapGenerator_Boss mapGenerator;

    private int _health;

    const int LIMIT = 1000;
    public int health {
        get { return _health; }
        set
        {
            healthBar.setHealth(value);
            _health = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar.setMaxHealth(10);
        health = 10;
    }

    public void attack()
    {
        Debug.Log("attack()");

        int i, j, cnt = 0;

        do
        {
            i = Random.Range(0, gameManager.n);
            j = Random.Range(0, gameManager.m);
        } while ((gameManager.myMap[i, j].topLayer != ' ' ||
                 gameManager.myMap[i, j].groundLayer == 'X') &&
                 ++cnt != LIMIT);

        if (cnt == LIMIT)
        {
            Debug.Log("Full");
            return;
        }

        gameManager.myMap[i, j].topLayer = '!'; // pending

        Debug.Log("ij: " + i + " " + j);

        GameObject obj = mapGenerator.generatePrefabs(i-10, j, -0.5f, 0.05f, mapGenerator.prefabs[4]);

        gameManager.myMap[i, j].topLayer = 'M';
        gameManager.myMapObj[i, j].topLayer = obj;

        StartCoroutine(objFallDown(obj, i, j, 1f));
    }

    IEnumerator objFallDown(GameObject obj, int i, int j, float time)
    {
        Vector2 target = mapGenerator.getPos(i, j, -0.5f, 0.05f);
        float speed = Vector2.Distance(obj.transform.position, target) / time;

        while (obj.transform.position.y > target.y)
        {
            Debug.Log("result: " + obj.transform.position.y);
            obj.transform.position -= new Vector3(0f, speed * Time.fixedDeltaTime, 0f);
            yield return new WaitForFixedUpdate();
        }

        //obj.transform.position = target;
        //yield return new WaitForFixedUpdate();

        gameManager.myMap[i, j].topLayer = 'M';
    }
}
