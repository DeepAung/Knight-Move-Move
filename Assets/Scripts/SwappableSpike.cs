using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwappableSpike : MonoBehaviour
{
    public int[] position = new int[2];
    public bool startWith;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetBool("isActive", startWith);
    }
}
