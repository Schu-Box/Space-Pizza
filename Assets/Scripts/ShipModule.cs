using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipModule : MonoBehaviour
{
    public bool coreModule = false;
    public int health = 1;
    public int damageDealtOnCollision = 5;

    public List<ShipSubModule> shipSubModules = new List<ShipSubModule>();
    
    private List<ShipModule> _neighboringShipModules = new List<ShipModule>();

    private Ship ship;
    private SpriteRenderer _spriteRenderer;

    private Coroutine _explosionCoroutine;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ship = GetComponentInParent<Ship>();
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Hazard hazard = other.gameObject.GetComponent<Hazard>();
        if (hazard != null)
        {
            HitByHazard(hazard);
            hazard.TakeDamage(1);
        }
    }

    public void HitByHazard(Hazard hazard)
    {
        health -= hazard.damage;
        
        if (health <= 0)
        {
            DestroyShipModule();
        }
        else if (health <= 1)
        {
            ShowDamage();
        }
    }

    private void ShowDamage()
    {
        _spriteRenderer.color = Color.gray;
    }

    private void DestroyShipModule()
    {
        if (_explosionCoroutine == null)
        {
            ship.RemoveShipModule(this);

            _explosionCoroutine = StartCoroutine(ExplosionCoroutine());
        }
        else
        {
            Debug.LogWarning("Explosion is triggered but this thang is already exploding");
        }
      
    }

    public IEnumerator ExplosionCoroutine()
    {
        transform.SetParent(null);
        gameObject.AddComponent<Rigidbody2D>();
        
        //TODO: Apply force
        
        yield return new WaitForSeconds(0.5f);
        
        Destroy(gameObject);
    } 
}
