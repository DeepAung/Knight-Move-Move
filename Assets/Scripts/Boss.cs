using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public HealthBar healthBar;
    public GameManager_Boss gameManager;
    public MapGenerator_Boss mapGenerator;

    public Animator animator;

    // ----------------------------------- //
    public int stage;
    float movableStoneTime, spikeTime;

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

        healthBar.setMaxHealth(35);

        stage = 0;
    }

    private void Update()
    {
        if (health == 30)
        {
            stage = 1;
        }
        else if (health == 15)
        {
            stage = 2;
        }

        // ------------------------------------------------- //

        movableStoneTime -= Time.deltaTime;
        spikeTime -= Time.deltaTime;

        if (movableStoneTime <= 0)
        {
            animator.SetTrigger("Attack");
        }

        if (movableStoneTime <= 0)
        {
            AttackMovableStoneHandler();

            movableStoneTime = 2f;
        }

        if (spikeTime <= 0)
        {
            AttackSpikeHandler();

            if (stage == 0) spikeTime = 2.5f;
            else if (stage == 1) spikeTime = 1.5f;
            else if (stage == 2) spikeTime = 1.3f;
        }
    }

    void AttackMovableStoneHandler()
    {

        int i, j, cnt = 0;
        do
        {
            i = Random.Range(0, gameManager.n);
            j = Random.Range(0, gameManager.m);

            if (++cnt >= LIMIT) { Debug.Log("LIMIT"); return; }
        } while (gameManager.myMap[i, j].topLayer != ' ' || 
                 gameManager.myMap[i, j].groundLayer == 'E');

        StartCoroutine(attackMovableStone(i, j));
    }

    void AttackSpikeHandler()
    {

        bool rowOrCol;
        int index, amount = 1;

        if (stage == 0) amount = 1;
        else if (stage == 1) amount = 2;
        else if (stage == 2) amount = 3;

        for (int it = 0; it < amount; it++)
        {
            rowOrCol = Random.Range(0, 2) == 1;
            if (rowOrCol) index = Random.Range(0, gameManager.n);
            else index = Random.Range(0, gameManager.m);

            attackSpikeRowOrCol(rowOrCol, index);
        }
    }

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

    public IEnumerator attackMovableStone(int i, int j)
    {
        gameManager.myMap[i, j].topLayer = '!'; // pending

        GameObject obj = mapGenerator.generatePrefabs(i - 10, j, -0.5f, 0.05f, mapGenerator.prefabs[4]);

        obj.GetComponent<MovableStone>().position = new int[] { i, j };

        var collider = obj.GetComponent<BoxCollider2D>();
        collider.enabled = false;

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

        collider.enabled = true;

        yield return new WaitForSeconds(15f);

        if (gameManager.myMap[i, j].topLayer == ' ') yield break;

        gameManager.myMapObj[i, j].topLayer
            .GetComponent<Animator>()
            .SetTrigger("Destroy");

        Destroy(gameManager.myMapObj[i, j].topLayer, 1f);

        gameManager.myMap[i, j].topLayer = ' ';
        gameManager.myMapObj[i, j].topLayer = null;
    }

    public IEnumerator attackSpike(int i, int j)
    {
        GameObject obj = mapGenerator.generatePrefabs(i, j, -0.5f, 0.5f, mapGenerator.prefabs[7]);
        obj.GetComponent<Animator>().SetTrigger("Warning");

        gameManager.myMap[i, j].groundLayer = '!'; // pending

        yield return new WaitForSeconds(0.8f);

        gameManager.myMap[i, j].groundLayer = '|';
        gameManager.myMapObj[i, j].groundLayer = obj;

        yield return new WaitForSeconds(0.2f);

        // check if new spike can deal damage to player
        gameManager.canSpikeDealDamage(i, j);

        obj.GetComponent<Animator>().SetTrigger("Destroy");

        yield return new WaitForSeconds(0.5f);

        obj.GetComponent<NormalSpike>().Destroy();
        mapGenerator.generateTile(i, j, mapGenerator.tiles[1].tile);

        gameManager.myMap[i, j].groundLayer = '.';
        gameManager.myMapObj[i, j].groundLayer = null;
    }

}
