using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [System.Serializable]
    public struct pair
    {
        public KeyCode keyCode;
        public Button button;
    }

    public List<Button> buttons;
    public List<pair> customButtons;
    public int selection;

    // Start is called before the first frame update
    void Start()
    {
        selection = 0;
    }

    // Update is called once per frame
    void Update()
    {
        renderButton();

        checkInputButton();

        checkCustomButton();

    }

    void renderButton()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var colors = buttons[i].colors;
            var script = buttons[i].GetComponent<ButtonScript>();

            if (i == selection)
            {
                colors.normalColor = new Color(0.9f, 0.9f, 0.9f);
                script.setToNew();
            }
            else
            {
                colors.normalColor = new Color(1f, 1f, 1f);
                script.setToOld();
            }

            buttons[i].colors = colors;
        }
    }

    void checkInputButton()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || 
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            selection++;
            if (selection == buttons.Count) selection = 0;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || 
            Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selection--;
            if (selection == -1) selection = buttons.Count - 1;
        }

        if (buttons.Count == 0) return;
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            buttons[selection].onClick.Invoke();
        }
    }

    void checkCustomButton()
    {
        foreach (var buttonPair in customButtons)
        {
            if (Input.GetKeyDown(buttonPair.keyCode))
            {
                buttonPair.button.onClick.Invoke();
            }
        }

    }
}

