using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public HealthBar healthBar;
    public GameManager_Boss gameManager;
    public MapGenerator_Boss mapGenerator;

    public Animator animator;

    const int LIMIT = 1000;
    public int health {
        get { return healthBar.getHealth(); }
        set
        {
            healthBar.setHealth(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        healthBar.setMaxHealth(10);
    }

    private void Update()
    {
        if (health == 0)
        {
            // boss died and then load dialogue scene
        }
    }

    public IEnumerator attackMovableStone(int i, int j)
    {
        gameManager.myMap[i, j].topLayer = '!'; // pending

        GameObject obj = mapGenerator.generatePrefabs(i - 10, j, -0.5f, 0.05f, mapGenerator.prefabs[4]);

        Vector2 target = mapGenerator.getPos(i, j, -0.5f, 0.05f);
        float speed = Vector2.Distance(obj.transform.position, target);
        while (obj.transform.position.y > target.y)
        {
            obj.transform.position -= new Vector3(0f, speed * Time.fixedDeltaTime, 0f);
            yield return new WaitForFixedUpdate();
        }

        obj.transform.position = new Vector3(target.x, target.y, 0);

        gameManager.myMap[i, j].topLayer = 'M';
        gameManager.myMapObj[i, j].topLayer = obj;
    }

    public IEnumerator attackSpike(int i, int j)
    {
        GameObject obj = mapGenerator.generatePrefabs(i, j, -0.5f, 0.5f, mapGenerator.prefabs[7]);
        obj.GetComponent<Animator>().SetTrigger("Warning");

        yield return new WaitForSeconds(1f);

        gameManager.myMap[i, j].groundLayer = '|';
        gameManager.myMapObj[i, j].groundLayer = obj;

        gameManager.canSpikeDealDamage(i, j);

        yield return new WaitForSeconds(3f);

        obj.GetComponent<Animator>().SetTrigger("Destroy");

        yield return new WaitForSeconds(0.5f);

        obj.GetComponent<NormalSpike>().Destroy();
        mapGenerator.generateTile(i, j, mapGenerator.tiles[1].tile);

        gameManager.myMap[i, j].groundLayer = '.';
        gameManager.myMapObj[i, j].groundLayer = null;
    }

    //IEnumerator objFallDown(GameObject obj, int i, int j, float time)
    //{
    //    Vector2 target = mapGenerator.getPos(i, j, -0.5f, 0.05f);
    //    float speed = Vector2.Distance(obj.transform.position, target) / time;

    //    while (obj.transform.position.y > target.y)
    //    {
    //        obj.transform.position -= new Vector3(0f, speed * Time.fixedDeltaTime, 0f);
    //        yield return new WaitForFixedUpdate();
    //    }

    //    //obj.transform.position = target;
    //    //yield return new WaitForFixedUpdate();

    //    gameManager.myMap[i, j].topLayer = 'M';
    //}

    public void attackSpikeRowOrCol(bool rowOrCol, int index)
    {
        if (rowOrCol)
        {
            for (int x = 0; x < gameManager.m; x++)
            {
                StartCoroutine(attackSpike(index, x));
            }
        }
        else
        {
            for (int x = 0; x < gameManager.n; x++)
            {
                StartCoroutine(attackSpike(x, index));
            }
        }
    }
}
