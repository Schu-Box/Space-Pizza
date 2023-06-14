using System;
using GamePhases;
using UnityEngine;

namespace ShipParts
{
    public class ConstructionYardTutorialUi : MonoBehaviour
    {
        [SerializeField]
        private GameObject tutorialUiRoot;

        private void Start()
        {
            tutorialUiRoot.SetActive(PhaseManager.Current.IsFirstConstructionPhase);
        }
    }
}