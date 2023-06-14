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

    private void Start()
    {
        moduleColorController.ShowDamage();
    }

    public void HitByHazard(Hazard hazard)
    {
        hazard.TakeDamage(damageDealtOnCollision);
        
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
        moduleColorController.ShowDamage();
    }

    private void DestroyShipModule()
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
}