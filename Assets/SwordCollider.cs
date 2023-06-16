using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name + " ENTERED");

        ShipModule shipModule = other.GetComponentInParent<ShipModule>();
        if (shipModule != null)
        {
            Debug.Log("Ship got hit");
            shipModule.TakeDamage(1);
        }

        Hazard hazard = other.GetComponentInParent<Hazard>();
        if (hazard != null)
        {
            Debug.Log("TOOK DAMAGE");
            hazard.TakeDamage(1);
        }
    }
}
