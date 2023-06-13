using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hazard : MonoBehaviour
{
    public int damage = 1;
    public int health = 1;
    public Vector2 speedRange = new Vector2(3f, 8f);

    public float explosionDuration = 0.4f;

    public List<Sprite> possibleSprites;

    public SpriteRenderer spriteRenderer;
    public Collider2D collider;
    public ParticleSystem destructionParticles;
    
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (possibleSprites.Count > 0)
        {
            spriteRenderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];
        }
    }

    public void SetTrajectory(Vector3 targetPosition)
    {
        if(_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();
        _rigidbody.velocity = direction * Random.Range(speedRange.x, speedRange.y);
        
        // Debug.Log(rigidbody.velocity);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        ShipModule shipModule = other.gameObject.GetComponent<ShipModule>();
        if (shipModule != null)
        {
            shipModule.HitByHazard(this);
            return;
        }

        Laser laser = other.gameObject.GetComponent<Laser>();
        if (laser != null)
        {
            TakeDamage(laser.damage);
            laser.DestroyLaser();
            return;
        }

        ShipShield shipShield = other.gameObject.GetComponent<ShipShield>();
        if (shipShield != null)
        {
            TakeDamage(shipShield.damageDealtOnCollision);
            shipShield.AbsorbDamage(damage);
            return;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyHazard();
        }
    }

    public void DestroyHazard()
    {
        collider.enabled = false;
        _rigidbody.velocity = Vector2.zero;
        spriteRenderer.gameObject.SetActive(false);
        if (destructionParticles != null)
        {
            destructionParticles.gameObject.SetActive(true);
        }
        
        StartCoroutine(DestroyHazardCoroutine());
    }

    private IEnumerator DestroyHazardCoroutine()
    {
        yield return new WaitForSeconds(explosionDuration);
        
        Destroy(this.gameObject);
    }
}
