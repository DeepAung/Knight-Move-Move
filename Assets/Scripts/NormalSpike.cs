using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSpike : MonoBehaviour
{
    public int[] position = new int[2];

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
