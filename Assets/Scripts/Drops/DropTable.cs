using System.Collections;
using System.Collections.Generic;
using System.Security;
using Drops;
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
        
        int totalWeight = 0;

        foreach (WeightedPrefab weightedPrefab in drops)
        {
            totalWeight += weightedPrefab.Weight;
        }

        float randomSelection = Random.value * totalWeight;

        foreach (WeightedPrefab weightedPrefab in drops)
        {
            if (randomSelection <= weightedPrefab.Weight)
            {
                dropPrefab = weightedPrefab.Prefab;
                return true;
            }

            randomSelection -= weightedPrefab.Weight;
        }

        dropPrefab = drops[0].Prefab;
        return true;
    }
}
