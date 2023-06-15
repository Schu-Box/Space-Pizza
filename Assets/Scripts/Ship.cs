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

    private bool jumpDriveCharging = false;
    private bool jumpDriveReady = false;
    private float jumpDriveChargeDuration = 45f;
    private float jumpDriveChargeTimer = 0f;

    private float totalWeight = 0f;
    private float speed = 0f;
    private float rotationSpeed = 0f;

    private bool isThrusting = false;

    private List<ShipModule> shipModules = new List<ShipModule>();

    public Vector2 Velocity => rb.velocity;

    private ShipModule coreModule;

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

        ResetJumpDrive();

        PhaseManager.Current.PhaseChangedEvent += ResetJumpDrive;
    }

    public void StartChargingJumpDrive()
    {
        jumpDriveCharging = true;
    }

    public void ResetJumpDrive()
    {
        jumpDriveReady = false;
        jumpDriveChargeTimer = 0f;
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
                            shipThruster.ToggleParticleLights(true);
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
                            shipThruster.ToggleParticleLights(false);
                        }
                    }
                }
            }
        }

        rb.angularVelocity = -Input.GetAxis("Horizontal") * rotationSpeed;

        if (!jumpDriveReady)
        {
            if (!jumpDriveCharging)
            {
                return;
            }
            
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
        
        PhaseManager.Current.PhaseChangedEvent -= ResetJumpDrive;
    }

    public void AddModule(ShipModule shipModule)
    {
        if (shipModules.Contains(shipModule))
        {
            return;
        }

        if (shipModule.coreModule)
        {
            if (coreModule != null)
            {
                Debug.LogError($"[Ship] Adding more than one core module!", shipModule);
            }
            
            coreModule = shipModule;
        }

        shipModule.ModuleDestroyedEvent += RemoveModule;
        shipModules.Add(shipModule);

        shipModule.RootTransform.parent = moduleParent;
        CaclulcateShipStats();

        SetUpNeighbors(shipModule);
    }

    public void RemoveModule(ShipModule shipModule, bool wasChainReaction)
    {
        shipModule.ModuleDestroyedEvent -= RemoveModule;
        shipModules.Remove(shipModule);

        if (shipModule.coreModule)
        {
            DestroyShip();
            return;
        }

        if(!wasChainReaction)
        {
            // in a chain reaction, only the first destroyed module needs to run this code 
            RemoveDisconnectedNeighboringShipModules(shipModule);
        }
        
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

    private float speedLostPerWeight = 0.1f;
    private float rotationSpeedLostPerWeight = 2f;
    
    public void CaclulcateShipStats()
    {
        totalWeight = 0f;
        speed = 0f;
        rotationSpeed = 0f;
        foreach (ShipModule shipModule in shipModules)
        {
            foreach (ShipSubModule shipSubModule in shipModule.shipSubModules)
            {
                if (shipSubModule is ShipThruster shipThruster)
                {
                    speed += shipThruster.Speed;
                    rotationSpeed += shipThruster.RotationSpeed;
                }
            }

            totalWeight += shipModule.weight;
        }

        speed =  speed - (totalWeight * speedLostPerWeight);
        rotationSpeed = rotationSpeed - (totalWeight - rotationSpeedLostPerWeight);
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

    private void RemoveDisconnectedNeighboringShipModules(ShipModule destroyedModule)
    {
        if (coreModule == null)
        {
            Debug.LogError($"[Ship] RemoveDisconnectedNeighboringShipModules called but" +
                           $" no core modules in known!");
            return;
        }
        
        // remove the destroyed module from the their neighbor's neighbor list
        foreach (ShipModule neighborModule in destroyedModule.NeighboringShipModules)
        {
            neighborModule.RemoveNeighbor(destroyedModule);
        }
        
        // find all modules connected to the core and remove all other modules
        HashSet<ShipModule> modulesConnectedToCore = new();

        List<ShipModule> nextModules = new() { coreModule };

        while (nextModules.Count > 0)
        {
            ShipModule moduleToCheck = nextModules[0];
            nextModules.RemoveAt(0);

            modulesConnectedToCore.Add(moduleToCheck);

            foreach (ShipModule neighborModule in moduleToCheck.NeighboringShipModules)
            {
                if (modulesConnectedToCore.Contains(neighborModule))
                {
                    // module already checked
                    continue;
                }

                if (nextModules.Contains(neighborModule))
                {
                    // module already scheduled to be checked
                    continue;
                }
                
                nextModules.Add(neighborModule);
            }
        }

        List<ShipModule> moduleCopy = new List<ShipModule>(shipModules);
        
        foreach(ShipModule shipModule in moduleCopy)
        {
            if (shipModule.coreModule)
            {
                continue;
            }
            
            if (modulesConnectedToCore.Contains(shipModule))
            {
                continue;
            }
            
            shipModule.DestroyShipModule();
        }
    }
}
