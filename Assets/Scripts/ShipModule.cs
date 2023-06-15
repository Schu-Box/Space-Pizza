using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using ShipParts;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipModule : MonoBehaviour
{
    public event Action<ShipModule> ModuleDestroyedEvent; 

    [SerializeField]
    private Transform rootTransform;

    public Transform RootTransform => rootTransform;

    [FormerlySerializedAs("shipColorController")]
    [FormerlySerializedAs("placementVisualizer")]
    
    [SerializeField]
    private ModuleColorController moduleColorController;

    public ModuleColorController ModuleColorController => moduleColorController;

    [SerializeField]
    private ShipModuleDefinition _moduleDefinition;

    public ShipModuleDefinition ModuleDefinition => _moduleDefinition;

    [SerializeField]
    private ModuleMover _moduleMover;

    public ModuleMover ModuleMover => _moduleMover;

    public int[,] Shape { get; } = new int[5, 5];

    public List<(int, int)> ShapeAsList { get; } = new();

    public bool coreModule = false;
    public int health = 1;
    public int damageDealtOnCollision = 5;

    public List<ShipSubModule> shipSubModules = new List<ShipSubModule>();

    private List<ShipModule> _neighboringShipModules = new List<ShipModule>();

    private Coroutine _explosionCoroutine;

    private void Awake()
    {
        if (_moduleDefinition == null)
        {
            Debug.LogError($"[ShipPart] is missing its module definition!", this);
        }

        string shapeDefinition = _moduleDefinition.ShapeDefinition;

        string[] rows = shapeDefinition.Split(',');

        for (int i = 0; i < rows.Length; i++)
        {
            string row = rows[i];

            int parsedValidSymbols = 0;

            for (int j = 0; j < row.Length; j++)
            {
                char symbol = row[j];

                if (symbol != '0' && symbol != '1')
                {
                    continue;
                }

                if (symbol == '1')
                {
                    Shape[i, parsedValidSymbols] = 1;
                    ShapeAsList.Add((i, parsedValidSymbols));
                }

                parsedValidSymbols += 1;
            }
        }

        // TODO debug only, delete if things seem to work
        for (int i = 0; i < Shape.GetLength(0); i++)
        {
            string rowString = "";

            for (int j = 0; j < Shape.GetLength(1); j++)
            {
                rowString += Shape[i, j];
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Laser>())
        {
            Debug.Log("HIT!!!!");
            HitByProjectile(other.gameObject.GetComponent<Laser>());
        }
    }

    public void HitByProjectile(Laser projectile)
    {
        projectile.DestroyLaser();

        TakeDamage(projectile.damage);
    }


    public void HitByHazard(Hazard hazard)
    {
        hazard.TakeDamage(damageDealtOnCollision);
        
       TakeDamage(hazard.damage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

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
        moduleColorController.ShowDamage();
    }

    public void DestroyShipModule()
    {
        if (_explosionCoroutine == null)
        {
            ModuleDestroyedEvent?.Invoke(this);
            
            _explosionCoroutine = StartCoroutine(ExplosionCoroutine());
        }
        else
        {
            Debug.LogWarning("Explosion is triggered but this thang is already exploding");
        }
    }

    public IEnumerator ExplosionCoroutine()
    {
        rootTransform.SetParent(null);
        rootTransform.gameObject.AddComponent<Rigidbody2D>();

        //TODO: Apply force

        yield return new WaitForSeconds(0.5f);

        Destroy(rootTransform.gameObject);
    }

    public void AddNeighbor(ShipModule neighbor)
    {
        if (_neighboringShipModules.Contains(neighbor))
        {
            return;
        }
        
        _neighboringShipModules.Add(neighbor);
    }

    public bool IsConnectedToCoreModule()
    {
        List<ShipModule> connectedModules = new List<ShipModule>();
        
        //TODO: Make this work!
        
        //loop through all neighbors of this shipModule and all neighbors of those neighbors and so on
        //if a neighbor is a core module, return true
        //if no core module is found, return false
        foreach (ShipModule neighbor in _neighboringShipModules)
        {
            if (!connectedModules.Contains(neighbor))
            {
                connectedModules.Add(neighbor);
                
                //and add all neighbors of neighbors and so on that haven't been added to modulestocheck yet
                foreach (ShipModule neighborOfNeighbor in neighbor._neighboringShipModules)
                {
                    if (!connectedModules.Contains(neighborOfNeighbor))
                    {
                        connectedModules.Add(neighborOfNeighbor);
                    }
                }
            }
        }

        foreach (ShipModule module in connectedModules)
        {
            if (module.coreModule)
            {
                return true;
            }
        }
        
        //TODO: Fix 

        return true;

        return false;
    }
}