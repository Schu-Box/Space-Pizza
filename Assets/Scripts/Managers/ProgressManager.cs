using System;
using UnityEngine;

namespace Managers
{
    public class ProgressManager: MonoBehaviour
    {
        public static ProgressManager Current => GameManager.Instance.
            ReferenceProvider.ProgressManager;

        public event Action CurrentLevelChangedEvent;

        public int CurrentLevel { get; private set; } = 0;

        public void HandleLevelCompleted()
        {
            CurrentLevel += 1;

            CurrentLevelChangedEvent?.Invoke();
        }
    }
}