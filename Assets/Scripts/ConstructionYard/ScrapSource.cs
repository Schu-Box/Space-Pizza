using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using ShipParts;
using UnityEngine;

public class ScrapSource : MonoBehaviour
{
    [SerializeField] 
    private DropTable scrapDrops;

    [SerializeField]
    private Transform scrapStartPosition;
    
    [SerializeField]
    private Transform scrapSink;

    [SerializeField]
    private float timeBetweenScrapSpawns;

    [SerializeField]
    private float scrapMoveDuration;

    private void Start()
    {
        StartCoroutine(SpawnScrap());
    }

    private IEnumerator SpawnScrap()
    {
        WaitForSeconds waitTime = new WaitForSeconds(timeBetweenScrapSpawns);
        
        while (true)
        {
            if (!scrapDrops.TrySelectDrop(out GameObject selectedScrapPrefab))
            {
                Debug.LogError("Could not select a scrap prefab!!!");
                yield break;
            }

            GameObject scrapObject = Instantiate(selectedScrapPrefab, scrapStartPosition.position,
                Quaternion.identity);

            ShipModule instantiatedPart = scrapObject.GetComponentInChildren<ShipModule>();

            if (instantiatedPart == null)
            {
                Debug.LogError("[ScrapPile] Spawned a scrap part that has no" +
                               " ship part component on it!");
                yield break;
            }
            
            instantiatedPart.ModuleMover.StartMoving(scrapStartPosition.position, scrapSink.position,
                scrapMoveDuration);
            
            yield return waitTime;
        }
    }
    
}
