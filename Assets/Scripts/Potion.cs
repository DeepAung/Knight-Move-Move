using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{

    public int power;

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
