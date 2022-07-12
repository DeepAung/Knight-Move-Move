using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // TODO: use stack to collect list of move that is on process
    [HideInInspector]
    public int moveCount;
    [HideInInspector]
    public Vector2Int pos;

    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;

    // --------------------------------------------------------------- //

    Vector2 movement;
    int frameCnt = 5;
    Queue<Vector2> myQueue;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10f;
        myQueue = new Queue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        playerAnimationController();

        playerInputKeyboard();

    }

    private void FixedUpdate()
    {

        playerMoveKeyboard();

    }

    void playerAnimationController()
    {
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (myQueue.Count == 0) return;

        Vector2 dummy = myQueue.Peek();
        if (dummy.x == 0) return;

        animator.SetFloat("Face", dummy.x);
        animator.SetFloat("Horizontal", dummy.x);

    }

    void playerInputKeyboard()
    {
        if (Input.GetButtonDown("Horizontal") && movement.x != 0)
        {
            myQueue.Enqueue(new Vector2(movement.x, 0f));
        }
        else if (Input.GetButtonDown("Vertical") && movement.y != 0)
        {
            myQueue.Enqueue(new Vector2(0f, movement.y));
        }
    }

    void playerMoveKeyboard()
    {

        if (myQueue.Count == 0) return;

        rb.MovePosition(rb.position + myQueue.Peek() * moveSpeed * Time.fixedDeltaTime);
        frameCnt--;
        moveCount--;

        //gameObject.layer += (int)myQueue.Peek().y;

        if (frameCnt == 0)
        {
            myQueue.Dequeue();
            frameCnt = 5;
        }

        if (moveCount == 0)
        {
            Destroy(gameObject);
        }
    }

}
