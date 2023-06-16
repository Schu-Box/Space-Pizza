using System;
using Helpers;
using Managers;
using UnityEngine;

namespace Drops
{
    [Serializable]
    public struct WeightedPrefab
    {
        [SerializeField] private AnimationCurve weightOverTime;

        [SerializeField] private GameObject prefab;

        public AnimationCurve WeightOverTime => weightOverTime;

        public GameObject Prefab => prefab;
        public float CurrentWeight => weightOverTime.EvaluateLimitless(ProgressManager.Current.CurrentLevel);
    }
}