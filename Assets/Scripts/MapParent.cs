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
    MapButton[] buttons;

    // for toRoman function
    static string[] romanLetter = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I", };
    static int[] romanNumber = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

    // Start is called before the first frame update
    void Start()
    {
        buttons = new MapButton[10];
        mapList = PassValue.instance.mapList;

        maxPage = (mapList.Count + 9) / 10; // always round up
        remainder = mapList.Count % 10; // the left over
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
        for (int i = 0; i < 10; i++) if (buttons[i]) buttons[i].Destroy();

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
                    new Vector3((float)j*300, (float)i*125, 0f), 
                    Quaternion.identity
                );
                button.transform.SetParent(gameObject.transform, false);
                button.name = mapList[cnt].ToString();

                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                buttonText.text = (mapList[cnt] == 0) ? "O" : toRoman(mapList[cnt]);

                // ----------- //
                buttons[cnt - firstIndex] = button.GetComponent<MapButton>();

                cnt++;
            }
        }
    }

    public void goLeftPage()
    {
        if (firstIndex == 0) return;
        firstIndex -= 10;
        renderMapButton();
    }

    public void goRightPage()
    {
        if (firstIndex + 10 > mapList.Count) return;
        firstIndex += 10;
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
