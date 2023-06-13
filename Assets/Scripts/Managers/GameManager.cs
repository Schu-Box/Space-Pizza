using System;
using UnityEngine;

namespace Managers
{
    public class GameManager: MonoBehaviour
    {
        public static GameManager Instance = null;

        [SerializeField] 
        private ReferenceProvider referenceProvider;

        public ReferenceProvider ReferenceProvider => referenceProvider;

        private void Awake()
        {
            if (Instance != null &&
                Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}