using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipModule : MonoBehaviour
{
    public int health = 1;
    public int damageDealtOnCollision = 5;

    public List<ShipSubModule> shipSubModules = new List<ShipSubModule>();
    
    private List<ShipModule> neighboringShipModules = new List<ShipModule>();

    private Ship ship;
    private SpriteRenderer _spriteRenderer;
    
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
        Destroy(gameObject);
        ship.RemoveShipModule(this);
    }
}
