using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class HazardManager : MonoBehaviour
{
   public static HazardManager Instance;
   
   public Transform hazardParent;
   public Hazard coinPrefab;

   public Vector2 spawnRadiusRange = new Vector2(35f, 55f);

   public Vector2 targetRadiusRange = new Vector2(0f, 20f);

   [SerializeField]
   private DropTable enemyComposition;
   
   [SerializeField]
   private AnimationCurve spawnRateOverTime;

   private float currentSpawnRate = 1f;
   
   private float _spawnTimer = 0f;
   
   private bool spawningHazards = false;
   
   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      ProgressManager.Current.CurrentLevelChangedEvent += UpdateSpawnRate;
      UpdateSpawnRate();
   }

   public void Update()
   {
      if(!spawningHazards)
      {
         return;
      }
      
      _spawnTimer += Time.deltaTime;
      if (_spawnTimer >= currentSpawnRate)
      {
         SpawnHazard();
      }
   }

   private void SpawnHazard()
   {
      _spawnTimer = 0f;
      Vector2 randomPosition = Random.insideUnitCircle.normalized * Random.Range(spawnRadiusRange.x, spawnRadiusRange.y);

      if (!enemyComposition.TrySelectDrop(out GameObject hazardPrefab))
      {
         Debug.LogError($"[HazardManager] Unable to select a hazard to spawn!");
         return;
      }

      Ship playerShip = ShipManager.Current.PlayerShip;
      
      if (playerShip == null)
      {
         return;
      }

      Hazard hazardObject = Instantiate(hazardPrefab, playerShip.RootTransform.position + (Vector3)randomPosition,
         Quaternion.identity, hazardParent).GetComponent<Hazard>();

      Vector3 targetPosition = playerShip.RootTransform.position +
                               (Random.insideUnitSphere * Random.Range(targetRadiusRange.x, targetRadiusRange.y));

      hazardObject.SetTrajectory(targetPosition);
   }

   private void OnDestroy()
   {
      ProgressManager.Current.CurrentLevelChangedEvent -= UpdateSpawnRate;
   }

   private void UpdateSpawnRate()
   {
      currentSpawnRate = spawnRateOverTime.EvaluateLimitless(ProgressManager.Current.CurrentLevel);
   }
   
   public void StartSpawningHazards()
   {
      spawningHazards = true;
   }
}
