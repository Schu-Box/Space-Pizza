using System;
using Managers;
using UnityEngine;

namespace ShipParts
{
    public class PartSelector: MonoBehaviour
    {
        [SerializeField] private ShipPart _shipPart;

        private void OnMouseDown()
        {
            DragAndDropManager.Current.StartDragging(_shipPart);
        }
    }
}