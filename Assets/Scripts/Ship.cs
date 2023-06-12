using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool addForceMovement = false;

    private float speed = 0f;
    private float rotationSpeed = 0f;
    
    private List<ShipModule> shipModules = new List<ShipModule>();
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        foreach (Transform child in transform)
        {
            ShipModule shipModule = child.GetComponent<ShipModule>();
            if (shipModule != null)
            {
                shipModules.Add(shipModule);
            }
        }
        
        CaclulcateShipStats();
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

        //TODO: Refactor, this is gross
        if (Input.GetButtonDown("Fire1"))
        {
            foreach (ShipModule shipModule in shipModules)
            {
                foreach (ShipSubModule subModule in shipModule.shipSubModules)
                {
                    if (subModule is ShipLaser)
                    {
                        ShipLaser shipLaser = subModule as ShipLaser;
                        shipLaser.FireLaser();
                    }
                }
            }
        }
    }
    
    public void CaclulcateShipStats()
    {
        speed = 0f;
        rotationSpeed = 0f;
        foreach (ShipModule shipModule in shipModules)
        {
            foreach (ShipSubModule shipSubModule in shipModule.shipSubModules)
            {
                if (shipSubModule is ShipThruster)
                {
                    ShipThruster shipThruster = shipSubModule as ShipThruster;
                    speed += shipThruster.speed;
                    rotationSpeed += shipThruster.rotationSpeed;
                }
            }
        }
    }
    
    public void RemoveShipModule(ShipModule shipModule)
    {
        shipModules.Remove(shipModule);
        
        //TODO: Also remove neighboring ship modules that are no longer connected to the core

        CaclulcateShipStats();
    }

    private void RemoveDisconnectedNeighboringShipModules()
    {
        
    }
}
