using GamePhases;
using UnityEngine;

namespace Helpers
{
    public class PhaseSwitcher: MonoBehaviour
    {
        [SerializeField]
        private GamePhase targetPhase = GamePhase.None;

        public void SwitchPhase()
        {
            PhaseManager.Current.SwitchPhase(targetPhase);
        }
    }
}