using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public ShipShield shipShield;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        Hazard hazard = other.gameObject.GetComponent<Hazard>();
        if (hazard != null)
        {
            hazard.TakeDamage(shipShield.damageDealtOnCollision);
            shipShield.AbsorbDamage(hazard.damage);
        }
    }
}
