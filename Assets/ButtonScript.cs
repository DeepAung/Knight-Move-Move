using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    Button button;
    public Sprite newImage;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void changeImage()
    {
        Debug.Log("on click");
        button.image.sprite = newImage;
        Debug.Log("end on click");
    }
}
