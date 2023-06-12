using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 1;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        Hazard hazard = other.gameObject.GetComponent<Hazard>();
        if (hazard != null)
        {
            hazard.TakeDamage(damage);
        }
    }
}
