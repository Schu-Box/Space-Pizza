using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private Transform rootTransform;

    public Transform RootTransform => rootTransform;

    [SerializeField]
    private Transform moduleParent;
    
    private Rigidbody2D rb;
    public bool addForceMovement = false;

    private float speed = 0f;
    private float rotationSpeed = 0f;

    private List<ShipModule> shipModules = new List<ShipModule>();

    private void Awake()
    {
        ShipManager.Current.RegisterShip(this);
    }

    private void Start()
    {
        DontDestroyOnLoad(rootTransform);
        
        rb = rootTransform.GetComponent<Rigidbody2D>();

        foreach (ShipModule shipModule in rootTransform.GetComponentsInChildren<ShipModule>())
        {
            if (shipModule != null && shipModule.gameObject.activeInHierarchy)
            {
                AddModule(shipModule);
            }
        }

        CaclulcateShipStats();
    }

    void Update()
    {
        if (addForceMovement)
        {
            float forwardInput = Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f);
            rb.AddForce(transform.up * (forwardInput * speed) / 10f);
        }
        else
        {
            rb.velocity = transform.up * (Input.GetAxis("Vertical") * speed);
        }

        rb.angularVelocity = -Input.GetAxis("Horizontal") * rotationSpeed;

        //TODO: Refactor, this is gross
        if (Input.GetButton("Fire1"))
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

        // if (Input.GetButtonDown("Fire2"))
        // {
        //     foreach (ShipModule shipModule in shipModules)
        //     {
        //         foreach (ShipSubModule subModule in shipModule.shipSubModules)
        //         {
        //             if (subModule is ShipShield)
        //             {
        //                 ShipShield shipShield = subModule as ShipShield;
        //                 shipShield.TriggerShield();
        //             }
        //         }
        //     }
        // }
    }

    private void OnDestroy()
    {
        foreach (ShipModule shipModule in shipModules)
        {
            shipModule.ModuleDestroyedEvent -= RemoveModule;
        }
    }

    public void AddModule(ShipModule shipModule)
    {
        if (shipModules.Contains(shipModule))
        {
            return;
        }

        shipModule.ModuleDestroyedEvent += RemoveModule;
        shipModules.Add(shipModule);

        shipModule.RootTransform.parent = moduleParent;
        CaclulcateShipStats();
    }
    
    public void RemoveModule(ShipModule shipModule)
    {
        shipModule.ModuleDestroyedEvent -= RemoveModule;
        shipModules.Remove(shipModule);

        if (shipModule.coreModule)
        {
            DestroyShip();
            return;
        }

        RemoveDisconnectedNeighboringShipModules();
        CaclulcateShipStats();
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
    
    
    
    private void DestroyShip()
    {
        Destroy(gameObject);
        
        InterfaceManager.Instance.DisplayGameOver();
    }

    private void RemoveDisconnectedNeighboringShipModules()
    {
        //TODO: Iterate through all ship modules and remove any that are not connected to the core module
    }
}
