using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableStone : MonoBehaviour
{

    public int[] position = new int[2];

    float moveSpeed = 10f;

    bool trigger = false;

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public IEnumerator moveTo(float dx, float dy)
    {
        for (int i = 0; i < 5; i++)
        {
            if (trigger) yield break;

            transform.position += new Vector3(dx, dy, 0f) * moveSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDestroy()
    {
        trigger = true;
    }

}
