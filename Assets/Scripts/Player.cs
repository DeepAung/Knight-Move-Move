using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // TODO: use stack to collect list of move that is on process
    //[HideInInspector]
    public int moveCount;
    [HideInInspector]
    public int[] position;
    public bool pass = false;

    public float moveSpeed;
    public Animator animator;

    // --------------------------------------------------------------- //

    Vector2 movement;
    int frameCnt = 5;
    Queue<Vector3> myQueue;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10f;
        myQueue = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        playerAnimationController();

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

    void playerMoveKeyboard()
    {

        if (myQueue.Count == 0) return;

        Vector3 deltaVector = myQueue.Peek();

        gameObject.transform.position += deltaVector * moveSpeed * Time.fixedDeltaTime;
        frameCnt--;

        if (frameCnt <= 0)
        {
            gameObject.transform.position = targetPos;
            myQueue.Dequeue();
            frameCnt = 5;
        }

        if (!pass && moveCount <= 0)
        {
            animationDestroy();
        }
    }

    public void enqueueMove(float x, float y)
    {
        moveCount--;

        position[0] -= (int)y;
        position[1] += (int)x;
        targetPos = gameObject.transform.position + new Vector3(x, y, 0);
        myQueue.Enqueue(new Vector3(x, y, 0f));
    }

    public void animationDestroy()
    {

        if (animator.GetFloat("Face") >= 0f)
        {
            animator.Play("Player_Dead_Right");
        }
        else
        {
            animator.Play("Player_Dead_Left");
        }
        //Destroy(gameObject, 2.0f);

        StartCoroutine( restartScene() );
    }

    public IEnumerator animationHit()
    {
        yield return new WaitForEndOfFrame();

        if (animator.GetFloat("Face") >= 0f)
        {
            animator.Play("Player_Hit_Right");
        }
        else
        {
            animator.Play("Player_Hit_Left");
        }
    }

    public IEnumerator restartScene()
    {
        Debug.Log("restarting scene");

        yield return new WaitForSecondsRealtime(1.5f);
        Debug.Log("restarting scene2");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
