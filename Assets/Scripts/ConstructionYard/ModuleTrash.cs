using System;
using Managers;
using UnityEngine;

namespace ShipParts
{
    public class ModuleTrash: MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            ShipModule shipModule = other.GetComponent<ShipModule>();

            if (shipModule == null)
            {
                return;
            }

            // make sure this module is not dragged anymore
            DragAndDropManager.Current.StopDragging(shipModule);
            
            Destroy(shipModule.RootTransform.gameObject);
        }
    }
}