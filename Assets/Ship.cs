using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 5f;
    public float rotationSpeed = 200f;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        //move rb up down lef right with arrow input
        rb.velocity = new Vector2(0f, Input.GetAxis("Vertical")) * speed;
        rb.angularVelocity = -Input.GetAxis("Horizontal") * rotationSpeed;
    }
}
