using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Button button;
    public Sprite oldImage, newImage;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();

        if (oldImage == null) oldImage = Resources.Load<Sprite>("Button/Normal_Unpressed");
        if (newImage == null) newImage = Resources.Load<Sprite>("Button/Normal_Pressed");
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        button.image.sprite = newImage;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        button.image.sprite = oldImage;
    }
}
