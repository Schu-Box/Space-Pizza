using System;
using System.Collections;
using System.Collections.Generic;
using GamePhases;
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

    private bool isThrusting = false;

    private List<ShipModule> shipModules = new List<ShipModule>();

    private void Awake()
    {
        if (ShipManager.Current.PlayerShip != null)
        {
            Destroy(rootTransform.gameObject);
            return;
        }
        
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
    
    //TODO: Refactor shipModule searching (maybe just have a shared formula?)

    void Update()
    {
        if (PhaseManager.Current.CurrentPhase == GamePhase.Construction)
        {
            return;
        }
        
        if (addForceMovement)
        {
            float forwardInput = Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f);
            if (forwardInput > 0f)
            {
                Vector2 force = rootTransform.up * (forwardInput * speed);
                rb.AddForce(force);
                
                Debug.Log(force);

                if (!isThrusting)
                {
                    isThrusting = true;
                    //Activate all shipModules that are thrusters
                    foreach (ShipModule shipModule in shipModules)
                    {
                        foreach (ShipSubModule subModule in shipModule.shipSubModules)
                        {
                            if (subModule is ShipThruster)
                            {
                                ShipThruster shipThruster = subModule as ShipThruster;
                                shipThruster.EnableParticles(true);
                            }
                        }
                    }
                }
            }
            else
            {
                if (isThrusting)
                {
                    isThrusting = false;
                    
                    //Deactivate all shipModules that are thrusters
                    foreach (ShipModule shipModule in shipModules)
                    {
                        foreach (ShipSubModule subModule in shipModule.shipSubModules)
                        {
                            if (subModule is ShipThruster)
                            {
                                ShipThruster shipThruster = subModule as ShipThruster;
                                shipThruster.EnableParticles(false);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            rb.velocity = transform.up * (Input.GetAxis("Vertical") * speed);
        }

        rb.angularVelocity = -Input.GetAxis("Horizontal") * rotationSpeed;

        //TODO: Refactor, this is gross
        if (Input.GetButton("Fire1"))
        {
            GameplayInterfaceManager.Instance.ActivateJumpDrive();
            
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
                if (shipSubModule is ShipThruster shipThruster)
                {
                    speed += shipThruster.speed;
                    rotationSpeed += shipThruster.rotationSpeed;
                }
            }
        }

        Debug.Log(speed);
    }
    
    
    
    private void DestroyShip()
    {
        Destroy(gameObject);
        
        GameplayInterfaceManager.Instance.DisplayGameOver();
    }

    private void RemoveDisconnectedNeighboringShipModules()
    {
        //TODO: Iterate through all ship modules and remove any that are not connected to the core module
    }
}
