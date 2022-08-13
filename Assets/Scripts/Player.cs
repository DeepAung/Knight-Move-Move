using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //[HideInInspector]
    public int moveCount;
    [HideInInspector]
    public int[] position;
    public bool pass = false;

    public float moveSpeed;
    public Animator animator;

    // for Boss
    private int _health;
    public int health {
        get {
            return healthBar.getHealth();
        }
        set {
            healthBar.setHealth(value);
        }
    }
    public HealthBar healthBar;

    // --------------------------------------------------------------- //

    Vector2 movement;
    int frameCnt = 5;
    Queue<Vector3> myQueue;

    // Start is called before the first frame update
    void Start()
    {
        if (PassValue.instance.isBossScene)
        {
            healthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<HealthBar>();
            healthBar.setMaxHealth(100);
        }

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
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x != 0) animator.SetFloat("Face", movement.x);

    }

    void playerMoveKeyboard()
    {

        if (myQueue.Count == 0) return;

        Vector3 deltaVector = myQueue.Peek();

        gameObject.transform.position += deltaVector * moveSpeed * Time.fixedDeltaTime;
        frameCnt--;

        if (frameCnt <= 0)
        {
            myQueue.Dequeue();
            frameCnt = 5;
        }

        if (PassValue.instance.isBossScene)
        {
            if (health <= 0)
            {
                animationDestroy();
            }
        }
        else
        {
            if (!pass && moveCount <= 0)
            {
                animationDestroy();
            }
        }

    }

    public void getDamage(int num = 1)
    {
        if (PassValue.instance.isBossScene)
        {
            health -= num;
            if (health > 0) animator.SetTrigger("Hit");
        }
        else
        {
            moveCount -= num;
            if (moveCount > 0) animator.SetTrigger("Hit");
        }

        AudioManager.instance.play("SpikeHit");
    }

    public void enqueueMove(float x, float y)
    {
        position[0] -= (int)y;
        position[1] += (int)x;
        myQueue.Enqueue(new Vector3(x, y, 0f));
    }

    public void animationDestroy()
    {
        animator.SetTrigger("Dead");

        StartCoroutine( restartScene() );
    }

    public IEnumerator restartScene()
    {

        yield return new WaitForSeconds(1.5f);

        SceneLoader.instance.restartScene();
    }

}
