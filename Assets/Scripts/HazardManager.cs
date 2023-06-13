using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
   public Transform trackedObject;
   
   public Transform hazardParent;
   public Hazard asteroidPrefab;
   public Hazard coinPrefab;

   public Vector2 spawnRadiusRange = new Vector2(35f, 55f);

   public float spawnRate = 1f;
   private float _spawnTimer = 0f;

   public void Update()
   {
      _spawnTimer += Time.deltaTime;
      if (_spawnTimer >= spawnRate)
      {
         _spawnTimer = 0f;
         Vector2 randomPosition = Random.insideUnitCircle.normalized * Random.Range(spawnRadiusRange.x, spawnRadiusRange.y);

         Hazard hazardPrefab;
         float randomValue = Random.value;
         if (randomValue < 0.25f)
         {
            hazardPrefab = coinPrefab;
         }
         else
         {
            hazardPrefab = asteroidPrefab;
         }
         
         Hazard hazardObject = Instantiate(hazardPrefab, transform.position + (Vector3)randomPosition, Quaternion.identity, hazardParent).GetComponent<Hazard>();
         hazardObject.SetTrajectory(trackedObject.position);
      }
   }
}