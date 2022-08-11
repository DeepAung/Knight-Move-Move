using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableStone : MonoBehaviour
{

    public int[] position = new int[2];

    float moveSpeed = 10f;

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public IEnumerator moveTo(float dx, float dy)
    {
        for (int i = 0; i < 5; i++)
        {
            transform.position += new Vector3(dx, dy, 0f) * moveSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

}
