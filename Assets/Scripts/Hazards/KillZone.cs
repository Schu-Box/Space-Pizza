using System;
using UnityEngine;

namespace Hazards
{
    public class KillZone: MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            ShipModule shipModule = other.GetComponent<ShipModule>();

            if (shipModule == null)
            {
                return;
            }
            
            shipModule.DestroyShipModule();
        }
    }
}