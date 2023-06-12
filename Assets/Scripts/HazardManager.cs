using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
   public Transform trackedObject;
   
   public Transform hazardParent;
   public Asteroid asteroidPrefab;

   public float spawnRadius = 30f;

   public float spawnRate = 1f;
   private float spawnTimer = 0f;

   public void Update()
   {
      //Instantiate a new asteroid at a random position on the spawn radiuss

      spawnTimer += Time.deltaTime;
      if (spawnTimer >= spawnRate)
      {
         spawnTimer = 0f;
         Vector2 randomPosition = Random.insideUnitCircle.normalized * spawnRadius;
         Asteroid asteroid = Instantiate(asteroidPrefab, randomPosition, Quaternion.identity, hazardParent).GetComponent<Asteroid>();
         asteroid.SetTrajectory(trackedObject.position);
      }
   }
}
