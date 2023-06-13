using System.Collections;
using System.Collections.Generic;
using Managers;
using ShipParts;
using UnityEngine;

public class ScrapPile : MonoBehaviour
{
    [SerializeField] 
    private DropTable scrapDrops;
    
    private void OnMouseDown()
    {
        if (!scrapDrops.TrySelectDrop(out GameObject selectedScrapPrefab))
        {
            Debug.LogError("Could not select a scrap prefab!!!");
            return;
        }

        GameObject scrapObject = Instantiate(selectedScrapPrefab, Vector3.zero, Quaternion.identity);

        ShipModule instantiatedPart = scrapObject.GetComponentInChildren<ShipModule>();

        if (instantiatedPart == null)
        {
            Debug.LogError("[ScrapPile] Spawned a scrap part that has no" +
                           " ship part component on it!");
        }
        
        DragAndDropManager.Current.StartDragging(instantiatedPart);
    }
}
