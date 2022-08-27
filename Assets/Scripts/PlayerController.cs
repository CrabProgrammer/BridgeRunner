using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoxCollider2D playerCollider;
    private Rigidbody2D playerRigidBody;
    private Animator animator;
    private GameManager gameManager;
    private bool canStop;

    private Vector3 defaultPosition;

    void Awake()
    {
        defaultPosition = transform.position; //save start position
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        if (transform.position.y < defaultPosition.y)  //if falling
        {
            gameManager.ChangeState(GameManager.GameState.GameOver);
        }
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border")) //if collides with border of block(collider)
        {
            collision.gameObject.SetActive(false); //disable collider
            if (canStop) //if player cannot fall after bridge
            {
                gameManager.ChangeState(GameManager.GameState.Building);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ScoreZone")) //trigger zone above
        {
            collision.gameObject.SetActive(false);
            gameManager.IncreaseScore();
        }

    }
    public void ResetPosition()
    {
        playerRigidBody.simulated = false; // player cannot fall
        transform.position = defaultPosition;
    }

    public bool IsOnGround() // if raycast detects ground
    {
        LayerMask mask = LayerMask.GetMask("Ground"); 
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.0f, mask))
        {
            playerRigidBody.simulated = true; //can fall
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Run(bool canStop)
    {
        this.canStop = canStop;
        animator.Play("PlayerRun");
    }
    public void Stay()
    {
        animator.Play("PlayerIdle");
    }
    public float GetRightBoundPosition() //x position for bridge spawning 
    {
        return transform.position.x - transform.localScale.x / 2 + playerCollider.size.x;
    }
}
