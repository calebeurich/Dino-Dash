﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GenerateScript generate;
    public float movementSpeed = 3f;
    public float jumpVelocity = 100f;
    public int dinoHunger = 100;
    private bool jumping = false;
    private bool grounded = false;

    private DinoAnimator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    private void Start()
    {
        generate.SpawnWave();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<DinoAnimator>();
        audioSource = GetComponent<AudioSource>();
        if(Settings.instance != null)
        {
            animator.runningSprites = Settings.instance.GetRunningSprites();
        }
        animator.RunningAnimation();

        InvokeRepeating("HungerEnumerator", 0f, 0.5f);
    }

    void HungerEnumerator()
    {
        Debug.Log(dinoHunger);
        dinoHunger -= 1;
    }

    void Update()
    {
        if(dinoHunger >= 0)
        {
            UIManager.instance.UpdateHunger(dinoHunger);
        } else
        {
            GameManager.instance.isAlive = false;
        }
    }

    private void FixedUpdate()
    {
        //Touch input
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    //Touch left side of screen
                    Move(true);
                    animator.framesPerSecond = 30f;
                }
                else if (touch.position.x > Screen.width / 2)
                {
                    //Touch right side of screen
                    generate.SpawnWave();
                }
            }
        } else
        {
            animator.framesPerSecond = 20f;
            Move(false);
        }
    }

    private void Move(bool moveForward)
    {
        if(grounded)
        {
            Vector2 movement = new Vector2(0, 0);

            if (moveForward)
            {
                movement.x = movementSpeed;
            }
            else
            {
                movement.x = -movementSpeed;
            }

            if (jumping)
            {
                movement.y = jumpVelocity;
            }

            rb.velocity = movement;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (collisionObject.CompareTag("Bird"))
        {
            if(collisionObject.GetComponent<Obstacle>().cooked)
            {
                audioSource.Play();
                Destroy(collision.gameObject);
                GameManager.instance.score += 1;
                dinoHunger += 25;
            }
            else
            {
                audioSource.Play();
                Destroy(collision.gameObject);
                dinoHunger -= 5;
            }

        }

        if(collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }

        if (collisionObject.CompareTag("fire"))
        {
            dinoHunger -= 15;
            //play charring sound also here
            audioSource.Play();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bump"))
        {
            jumping = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
