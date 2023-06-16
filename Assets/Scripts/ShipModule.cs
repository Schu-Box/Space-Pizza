using System;
using System.Collections;
using System.Collections.Generic;
using GamePhases;
using ShipParts;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipModule : MonoBehaviour
{
    public event Action HealthChangedEvent;

    public event Action<ShipModule, bool> ModuleDestroyedEvent;

    [SerializeField]
    private Transform rootTransform;

    [SerializeField]
    private GameObject visualsRoot;
    
    [SerializeField] 
    private Transform visualCenterPoint;

    // public Animator explosionAnimator;
    public GameObject explosionAnimationPrefab;
    public GameObject damageAnimationPrefab;

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

    public float weight = 1f;

    [SerializeField]
    [FormerlySerializedAs("health")]
    private int maxHealth = 1;

    public int MaxHealth => maxHealth;

    public int CurrentHealth
    {
        get => currentHealthInternal;

        set
        {
            if (value == currentHealthInternal)
            {
                return;
            }

            currentHealthInternal = value;
            HandleHealthChange();
        }
    }

    private int currentHealthInternal = 999;

    public int damageDealtOnCollision = 5;

    public List<ShipSubModule> shipSubModules = new List<ShipSubModule>();
    
    public List<ShipModule> NeighboringShipModules { get; } = new List<ShipModule>();

    private Coroutine _explosionCoroutine;

    private List<GameObject> damageEffects = new();

    private void Awake()
    {
        if (_moduleDefinition == null)
        {
            Debug.LogError($"[ShipPart] is missing its module definition!", this);
        }

        CurrentHealth = maxHealth;

        ParseShapeInformation();

        PhaseManager.Current.PhaseChangedEvent += HandlePhaseChange;
        
        // explosionAnimator.Play("ModuleExplosion");
    }

    private void OnDestroy()
    {
        PhaseManager.Current.PhaseChangedEvent -= HandlePhaseChange;
    }

    private void HandlePhaseChange()
    {
        if (PhaseManager.Current.CurrentPhase != GamePhase.Construction)
        {
            return;
        }

        CurrentHealth = maxHealth;
    }

    private void ParseShapeInformation()
    {
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
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("Hit by object " + other.gameObject.name);
        
        if (other.gameObject.GetComponent<Laser>())
        {
            HitByProjectile(other.gameObject.GetComponent<Laser>());
        }

        // if (other.gameObject.GetComponent<SwordCollider>())
        // {
        //     Debug.Log("hit by sword");
        // }
    }

    private void HandleHealthChange()
    {
        if (CurrentHealth == maxHealth)
        {
            // make sure there is no damage animation showing
            foreach (GameObject damageEffect in damageEffects)
            {
                if (damageEffect == null)
                {
                    continue;
                }
                
                Destroy(damageEffect);
            }
            
            damageEffects.Clear();
        }
        
        HealthChangedEvent?.Invoke();
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
        if (PhaseManager.Current.IsJumping)
        {
            // no damage during jumps
            return;
        }
        
        GameObject damageAnimation = Instantiate(damageAnimationPrefab, visualCenterPoint.position, 
            Quaternion.identity, visualCenterPoint);
        
        damageEffects.Add(damageAnimation);
        
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            DestroyShipModule();
        }
    }

    public void DestroyShipModule(bool isChainReaction = false)
    {
        if (_explosionCoroutine == null)
        {
            ModuleDestroyedEvent?.Invoke(this, isChainReaction);

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

        Instantiate(explosionAnimationPrefab, visualCenterPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);

        Destroy(rootTransform.gameObject);
    }

    public void AddNeighbor(ShipModule neighbor)
    {
        if (NeighboringShipModules.Contains(neighbor))
        {
            return;
        }

        NeighboringShipModules.Add(neighbor);
    }

    public void HandleModuleGrabbed()
    {
        if (PhaseManager.Current.CurrentPhase == GamePhase.Construction && ConstructionInterfaceManager.Instance.BuildTutorialShown)
        {
            ConstructionInterfaceManager.Instance.HideBuildTutorial();
        }
        
        foreach (ShipSubModule subModule in shipSubModules)
        {
            subModule.HandleModuleGrabbed();
        }
    }

    public void RemoveNeighbor(ShipModule neighborToRemove)
    {
        NeighboringShipModules.Remove(neighborToRemove);
    }

    public void ChangeVisibility(bool isVisible)
    {
        visualsRoot.SetActive(isVisible);
    }
}