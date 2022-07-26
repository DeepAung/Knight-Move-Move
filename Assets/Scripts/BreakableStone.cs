using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableStone : MonoBehaviour
{

    public int[] position = new int[2];
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Destroy()
    {
        animator.Play("BreakableStone_ToBreak");
        Destroy(gameObject, 0.5f);
    }
}
