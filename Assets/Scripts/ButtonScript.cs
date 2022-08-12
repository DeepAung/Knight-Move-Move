using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Button button;
    public Sprite oldImage, newImage;
    public bool pressing = false;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();

        if (oldImage == null) oldImage = Resources.Load<Sprite>("Button/Normal_Unpressed");
        if (newImage == null) newImage = Resources.Load<Sprite>("Button/Normal_Pressed");
    }

    public void setToNew()
    {
        button.image.sprite = newImage;
    }

    public void setToOld()
    {
        if (pressing) return;
        button.image.sprite = oldImage;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        pressing = true;
        Debug.Log("pointer down");
        setToNew();
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        pressing = false;
        Debug.Log("pointer up");
        setToOld();
    }
}
