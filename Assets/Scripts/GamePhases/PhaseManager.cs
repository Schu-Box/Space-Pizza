using Managers;
using UnityEngine;

namespace GamePhases
{
    public class PhaseManager: MonoBehaviour
    {
        public static PhaseManager Current => GameManager.Instance.ReferenceProvider.PhaseManager;

        [SerializeField] private GamePhase currentPhase = GamePhase.Construction;

        public GamePhase CurrentPhase => currentPhase;
    }
}