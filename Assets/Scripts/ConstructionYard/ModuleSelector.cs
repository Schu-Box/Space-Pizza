using System;
using Managers;
using UnityEngine;

namespace ShipParts
{
    public class ModuleSelector: MonoBehaviour
    {
        [SerializeField] 
        private ShipModule _shipPart;

        private bool canBeSelected = true;

        public void HandleModuleSelected()
        {
            if (!canBeSelected)
            {
                return;
            }

            _shipPart.ModuleMover.StopMoving();
            
            DragAndDropManager.Current.StartDragging(_shipPart);

            canBeSelected = false;
        }
    }
}