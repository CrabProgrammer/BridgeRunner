using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float leftBound = 20f;
    GameManager gameManager;
    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private bool isBackground;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //speed of background twice slower for parallax effect
        if (isBackground)
        {
            speed /= 2;
        }
    }

    void Update()
    {
        if(gameManager.canMove) //if can move left in current GameState
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed,Space.World);
            if (transform.position.x < -leftBound)
            {
                Destroy(gameObject);
            }
        }
    }

}
    