using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapParent : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public GameObject arrowLeft, arrowRight;

    List<int> mapList;
    int maxPage, remainder;
    int firstIndex = 0;
    bool isLoad = false;
    // Start is called before the first frame update
    void Start()
    {
        mapList = PassValue.instance.mapList;

        maxPage = (mapList.Count + 19) / 20; // always round up
        remainder = mapList.Count % 20; // the left over
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLoad && gameObject.activeSelf)
        {
            renderMapButton();
            isLoad = true;
        }
    }

    void renderMapButton()
    {
        int cnt = firstIndex;
        for (int i = 1; i >= -1; i -= 2) // i = 1, -1
        {
            for (int j = -2; j <= 2; j++) // j = -2, -1, 0, 1, 2
            {
                //GameObject button Instantiate(ButtonPrefab.transform.position, ButtonPrefab);
                // new Vector3(j*350, i*150, 0)
                //button.transform.parent = gameObject.transform;
                //button.name = mapList[cnt++].ToString();
            }
        }
    }

    void goLeftPage()
    {
        if (firstIndex == 0) return;
        firstIndex -= 20;
        renderMapButton();
    }

    void goRightPage()
    {
        if (firstIndex + 20 > mapList.Count) return;
        firstIndex += 20;
        renderMapButton();
    }
}
