using System;
using System.Collections;
using System.Collections.Generic;
using GamePhases;
using Helpers;
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

    private bool jumpDriveReady = false;
    private float jumpDriveChargeDuration = 10f;
    private float jumpDriveChargeTimer = 0f;

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
                ShipGridController.Current.PlacePart(shipModule.transform.position.GridPosition(),
                    shipModule);
                
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

        rb.angularVelocity = -Input.GetAxis("Horizontal") * rotationSpeed;

        if (!jumpDriveReady)
        { 
            jumpDriveChargeTimer += Time.deltaTime;

            GameplayInterfaceManager.Instance.UpdateJumpDriveSlider(jumpDriveChargeTimer / jumpDriveChargeDuration);
        
            if (jumpDriveChargeTimer >= jumpDriveChargeDuration)
            {
                jumpDriveReady = true;
                GameplayInterfaceManager.Instance.DisplayJumpDriveReady();
            }
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                GameplayInterfaceManager.Instance.ActivateJumpDrive();
            }
        }
    }
    
    //TODO: Ask Hagen how he would do this
    // public List<ShipSubModule> GetShipSubModules(ShipSubModuleType??? )
    // {
    //     foreach (ShipModule shipModule in shipModules)
    //     {
    //         foreach (ShipSubModule subModule in shipModule.shipSubModules)
    //         {
    //             if (subModule is ShipThruster)
    //             {
    //                 ShipThruster shipThruster = subModule as ShipThruster;
    //                 shipThruster.EnableParticles(true);
    //             }
    //         }
    //     }
    // }

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

        SetUpNeighbors(shipModule);
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
    
    private void SetUpNeighbors(ShipModule shipModule)
    {
        List<ShipModule> neighbors = ShipGridController.Current.FindNeighbors(shipModule);

        foreach (ShipModule neighbor in neighbors)
        {
            shipModule.AddNeighbor(neighbor);
            neighbor.AddNeighbor(shipModule);
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
                if (shipSubModule is ShipThruster shipThruster)
                {
                    speed += shipThruster.speed;
                    rotationSpeed += shipThruster.rotationSpeed;
                }
            }
        }

        Debug.Log(speed);
    }

    public void StopPhysics()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void DestroyShip()
    {
        Destroy(gameObject);
        
        GameplayInterfaceManager.Instance.DisplayGameOver();
    }

    private void RemoveDisconnectedNeighboringShipModules()
    {
        //TODO: Iterate through all ship modules and remove any that are not connected to the core module
        
        foreach(ShipModule shipModule in shipModules)
        {
            if (shipModule.coreModule)
            {
                continue;
            }
            
            if (!shipModule.IsConnectedToCoreModule())
            {
                shipModule.DestroyShipModule();
                return;
            }
        }
    }
}
