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

    public IEnumerator attackMovableStone(int i, int j)
    {

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

    public IEnumerator attackSpike(bool rowOrCol, int index)
    {
        var objList = new List<GameObject>();
        var pairList = new List<int[]>();
        if (rowOrCol)
        {
            for (int x = 0; x < gameManager.m; x++)
            {
                GameObject obj = mapGenerator.generatePrefabs(index, x, -0.5f, 0.5f, mapGenerator.prefabs[7]);
                obj.GetComponent<Animator>().SetTrigger("Warning");
                objList.Add(obj);
                pairList.Add(new int[2] { index, x });
            }
        }
        else
        {
            for (int x = 0; x < gameManager.n; x++)
            {
                GameObject obj = mapGenerator.generatePrefabs(x, index, -0.5f, 0.5f, mapGenerator.prefabs[7]);
                obj.GetComponent<Animator>().SetTrigger("Warning");
                objList.Add(obj);
                pairList.Add(new int[] { x, index });
            }
        }

        yield return new WaitForSeconds(1f);

        int i = 0;
        foreach (var item in pairList)
        {
            gameManager.myMap[item[0], item[1]].groundLayer = '|';
            gameManager.myMapObj[item[0], item[1]].groundLayer = objList[i++];
        }

        // check if it can deal damage to player
        Debug.Log("1: " + rowOrCol + " " + index);
        gameManager.fromBossAttackSpike(rowOrCol, index);

        yield return new WaitForSeconds(3f);

        i = 0;
        foreach (var item in pairList)
        {
            objList[i++].GetComponent<Animator>().SetTrigger("Destroy");

        }

        yield return new WaitForSeconds(0.5f);

        i = 0;
        foreach (var item in pairList)
        {
            objList[i++].GetComponent<NormalSpike>().Destroy();
            mapGenerator.generateTile(item[0], item[1], mapGenerator.tiles[1].tile);

            gameManager.myMap[item[0], item[1]].groundLayer = '.';
            gameManager.myMapObj[item[0], item[1]].groundLayer = null;
        }
    }
}
