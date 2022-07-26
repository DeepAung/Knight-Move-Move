using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public int[] position = new int[2];

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
