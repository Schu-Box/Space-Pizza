using System;
using UnityEngine;

namespace Drops
{
    [Serializable]
    public struct WeightedPrefab
    {
        [SerializeField] private int weight;

        [SerializeField] private GameObject prefab;

        public int Weight => weight;

        public GameObject Prefab => prefab;
    }
}