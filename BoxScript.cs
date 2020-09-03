using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    private float min_X = -1.8f, max_X = 1.8f;

    private bool canMove;
    private float move_speed = 2.5f;

    private Rigidbody2D myBody;

    private bool gameOver;
    private bool ignoreCollision;
    private bool ignoreTrigger;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.gravityScale = 0f;
    }

    void Start()
    {
        canMove = true;

        if(UnityEngine.Random.Range(0,2) > 0)
        {
            move_speed *= -1f;
        }

        GameplayController.instance.currentBox = this;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        if (canMove)
        {
            Vector3 temp = transform.position;
            temp.x += move_speed * Time.deltaTime;

            if(temp.x > max_X)
            {
                move_speed *= -1f;
            }else if (temp.x < min_X)
            {
                move_speed *= -1f;
            }
                transform.position = temp;
        }
    }
    public void DropBox()
    {
        canMove = false;
        myBody.gravityScale = UnityEngine.Random.Range(2, 4);
    }
    void Landed()
    {
        if (gameOver)
            return;

        ignoreCollision = true;
        ignoreTrigger = true;

        GameplayController.instance.SpawnNewBox();
        GameplayController.instance.MoveCamera();
    }
    void RestartGame()
    {
        GameplayController.instance.RestartGame();
    }
    void OnCollisionEnter2D(Collision2D target)
    {
        if (ignoreCollision)
            return;
        if(target.gameObject.tag == "Platform")
        {
            Invoke("Landed", 0.8f);
            ignoreCollision = true;
        }
        if (target.gameObject.tag == "Box")
        {
            Invoke("Landed", 0.8f);
            ignoreCollision = true;
        }
    }
    void OnTriggerEnter2D(Collider2D target)
    {
        if (ignoreTrigger)
            return;
        if(target.tag == "Game Over")
        {
            CancelInvoke("Landed");
            gameOver = true;
            ignoreTrigger = true;

            Invoke("RestartGame", 0.5f);
        }
       // else if (target.tag == "Box" && target.tag == "Game Over")
       // {
         //   CancelInvoke("Landed");
           // gameOver = true;
           // ignoreTrigger = true;

            //Invoke("RestartGame", 0.5f);
        //}
        //else
        //{
          //  CancelInvoke("Landed");
            //gameOver = true;
           // ignoreTrigger = true;

            //Invoke("RestartGame", 0.5f);
        //}

    }
}
