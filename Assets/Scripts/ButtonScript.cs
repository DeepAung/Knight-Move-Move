using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
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
        setToNew();
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        pressing = false;
        setToOld();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.play("ButtonClick");
    }
}
