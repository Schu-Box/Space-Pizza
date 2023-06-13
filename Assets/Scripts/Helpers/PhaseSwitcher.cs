using GamePhases;
using UnityEngine;

namespace Helpers
{
    public class PhaseSwitcher: MonoBehaviour
    {
        [SerializeField]
        private GamePhase targetPhase = GamePhase.None;

        //TODO: Call this after timer
        
        public void SwitchPhase()
        {
            PhaseManager.Current.SwitchPhase(targetPhase);
        }
    }
}