using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class MapParent : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public GameObject arrowLeft, arrowRight;

    List<int> mapList;
    int maxPage, remainder;
    int firstIndex = 0;
    bool isLoad = false;

    // for toRoman function
    static string[] romanLetter = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I", };
    static int[] romanNumber = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

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
            Debug.Log("rendering map");
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
                if (cnt >= mapList.Count) // break
                {
                    i = -100;
                    break;
                }

                GameObject button = Instantiate(
                    ButtonPrefab, 
                    new Vector3((float)j*325, (float)i*125, 0f), 
                    Quaternion.identity
                );
                button.transform.SetParent(gameObject.transform, false);
                button.name = mapList[cnt].ToString();

                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                buttonText.text = (cnt == 0) ? "0" : toRoman(cnt);

                // ----------- //
                cnt++;
            }
        }
    }

    public void goLeftPage()
    {
        if (firstIndex == 0) return;
        firstIndex -= 20;
        renderMapButton();
    }

    public void goRightPage()
    {
        if (firstIndex + 20 > mapList.Count) return;
        firstIndex += 20;
        renderMapButton();
    }

    public static string toRoman(int num)
    {
        string romanStr = "";

        int i = 0;
        while (num > 0)
        {
            if (num >= romanNumber[i])
            {
                num -= romanNumber[i];
                romanStr += romanLetter[i];
            }
            else
            {
                i++;
            }
        }

        return romanStr;
    }
}
