using System.Collections;
using System.Collections.Generic;
using System.Security;
using Drops;
using Helpers;
using UnityEngine;

[CreateAssetMenu(menuName = "Drops/DropTable")]
public class DropTable : ScriptableObject
{
    [SerializeField] 
    private List<WeightedPrefab> drops = new List<WeightedPrefab>();

    public bool TrySelectDrop(out GameObject dropPrefab)
    {
        if (drops.Count == 0)
        {
            dropPrefab = null;
            return false;
        }
        
        float totalWeight = 0;

        foreach (WeightedPrefab weightedPrefab in drops)
        {
            totalWeight += weightedPrefab.CurrentWeight;
        }

        float randomSelection = Random.value * totalWeight;

        foreach (WeightedPrefab weightedPrefab in drops)
        {
            if (randomSelection <= weightedPrefab.CurrentWeight)
            {
                dropPrefab = weightedPrefab.Prefab;
                return true;
            }

            randomSelection -= weightedPrefab.CurrentWeight;
        }

        dropPrefab = drops[0].Prefab;
        return true;
    }
}
