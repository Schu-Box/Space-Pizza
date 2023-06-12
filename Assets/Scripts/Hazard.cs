using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public int damage = 1;

    public float speed = 1f;

    public bool destroyOnCollision = true;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetTrajectory(Vector3 targetPosition)
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();
        _rb.velocity = direction * speed;
    }
    
    public void HitShipModule()
    {
        if (destroyOnCollision)
        {
            Destroy(this.gameObject);
        }
    }
}
