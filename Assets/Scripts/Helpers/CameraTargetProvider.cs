using System;
using Cinemachine;
using Managers;
using UnityEngine;

namespace Helpers
{
    public class CameraTargetProvider: MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        private void Start()
        {
            Ship ship = ShipManager.Current.PlayerShip;

            if (ship == null)
            {
                Debug.LogError($"[CameraTargetProvider] Unable to retrieve player ship!");
                return;
            }

            _virtualCamera.Follow = ship.RootTransform;
        }
    }
}