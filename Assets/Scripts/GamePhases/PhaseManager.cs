using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePhases
{
    [Serializable]
    public struct SceneForPhase
    {
        public GamePhase phase;
        public string sceneName;
    }
    
    public class PhaseManager: MonoBehaviour
    {
        public static PhaseManager Current => GameManager.Instance.ReferenceProvider.PhaseManager;

        public event Action PhaseChangedEvent;
        
        [SerializeField] private GamePhase currentPhase = GamePhase.Construction;

        [SerializeField]
        private List<SceneForPhase> scenePhaseMapping = new();

        public GamePhase CurrentPhase => currentPhase;

        public bool IsFirstConstructionPhase { get; private set; } = true;

        public bool IsJumping { get; private set; } = false;

        public void ChangeJumpState(bool shouldJump)
        {
            IsJumping = shouldJump;
        }

        public void SwitchPhase(GamePhase nextPhase)
        {
            foreach (SceneForPhase sceneForPhase in scenePhaseMapping)
            {
                if (sceneForPhase.phase != nextPhase)
                {
                    continue;
                }
                
                SceneManager.LoadScene(sceneForPhase.sceneName);
            }

            if (currentPhase == GamePhase.Construction)
            {
                // left the construction phase
                IsFirstConstructionPhase = false;
            }
            
            currentPhase = nextPhase;
            PhaseChangedEvent?.Invoke();
        }
    }
}