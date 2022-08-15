using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapParent : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public GameObject arrowLeft, arrowRight;

    [SerializeField]
    static List<int> mapList
    {
        get { return PassValue.instance.mapList; }
        set { PassValue.instance.mapList = value; }
    }

    int firstIndex = 0;
    MapButton[] buttons;

    // for toRoman function
    static readonly string[] ROMAN_LETTER = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I", };
    static readonly int[] ROMAN_NUMBER = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

    // Start is called before the first frame update
    void Start()
    {
        buttons = new MapButton[10];

        loadMaps();
        renderMapButton();
    }

    // Update is called once per frame
    void Update()
    {

        if (firstIndex == 0) arrowLeft.SetActive(false);
        else arrowLeft.SetActive(true);

        if (firstIndex + 10 >= mapList.Count) arrowRight.SetActive(false);
        else arrowRight.SetActive(true);
    }

    public static void loadMaps()
    {
        mapList = new List<int>();

        string path = Application.streamingAssetsPath + "/Maps/";
        var dirInfo = new System.IO.DirectoryInfo(path);
        var files = dirInfo.GetFiles("*.txt");

        for (int i = 0; i < files.Length; i++)
        {
            bool canParse = int.TryParse(files[i].Name[0..^4], out int result);
            if (canParse)
            {
                mapList.Add(result);
            }
        }

        // boss map
        mapList.Add(int.MaxValue);

        mapList.Sort();
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
                if (mapList[cnt] == 0)
                {
                    buttonText.text = "O";
                }
                else if (mapList[cnt] == int.MaxValue) 
                {
                    buttonText.text = "BOSS";
                }
                else
                {
                    buttonText.text = toRoman(mapList[cnt]);
                }

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
            if (num >= ROMAN_NUMBER[i])
            {
                num -= ROMAN_NUMBER[i];
                romanStr += ROMAN_LETTER[i];
            }
            else
            {
                i++;
            }
        }

        return romanStr;
    }
}
