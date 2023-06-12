using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
   public Transform trackedObject;
   
   public Transform hazardParent;
   public Asteroid asteroidPrefab;

   public Vector2 spawnRadiusRange = new Vector2(35f, 55f);

   public float spawnRate = 1f;
   private float spawnTimer = 0f;

   public void Update()
   {
      spawnTimer += Time.deltaTime;
      if (spawnTimer >= spawnRate)
      {
         spawnTimer = 0f;
         Vector2 randomPosition = Random.insideUnitCircle.normalized * Random.Range(spawnRadiusRange.x, spawnRadiusRange.y);
         Asteroid asteroid = Instantiate(asteroidPrefab, transform.position + (Vector3)randomPosition, Quaternion.identity, hazardParent).GetComponent<Asteroid>();
         asteroid.SetTrajectory(trackedObject.position);
      }
   }
}
