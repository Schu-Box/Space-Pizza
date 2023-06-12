using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private Rigidbody2D rb;

    public bool addForceMovement = false;
    public float speed = 5f;
    public float rotationSpeed = 200f;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (addForceMovement)
        {
            rb.AddForce(transform.up * (Input.GetAxis("Vertical") * speed / 10f));
        }
        else
        {
            rb.velocity = transform.up * (Input.GetAxis("Vertical") * speed);
        }

        rb.angularVelocity = -Input.GetAxis("Horizontal") * rotationSpeed;
    }
}
