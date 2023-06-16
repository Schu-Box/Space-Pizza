using System;
using UnityEngine;

namespace Managers
{
    public class ProgressTracker: MonoBehaviour
    {
        public static ProgressTracker Current => GameManager.Instance.
            ReferenceProvider.ProgressTracker;

        public event Action CurrentLevelChangedEvent;

        public int CurrentLevel { get; private set; } = 0;

        public void HandleLevelCompleted()
        {
            CurrentLevel += 1;
            
            Debug.LogError($"Reached level {CurrentLevel}");
            
            CurrentLevelChangedEvent?.Invoke();
        }
    }
}