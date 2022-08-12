using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth(int health)
    {
        slider.value = health;
    }

    public int getHealth()
    {
        //Debug.Log("float vs int: " + slider.value + " | " + (int)slider.value);
        return (int)slider.value;
    }
}
