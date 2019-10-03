﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpScript : MonoBehaviour
{
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Dino"))
        {
            body = collision.gameObject.GetComponent<Rigidbody2D>();
            body.velocity = new Vector2(50f, 17f);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
