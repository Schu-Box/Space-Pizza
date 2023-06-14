using System;
using Helpers;
using ShipParts;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class DragAndDropManager: MonoBehaviour
    {
        public static DragAndDropManager Current => GameManager.Instance.ReferenceProvider.DragAndDropManager;
        
        private ShipModule _currentlyDraggedPart = null;

        private bool startedDraggingThisFrame = false;

        public void StartDragging(ShipModule objectToDrag)
        {
            if (_currentlyDraggedPart != null)
            {
                Debug.LogError("Already dragging something!");
                return;
            }

            _currentlyDraggedPart = objectToDrag;
            startedDraggingThisFrame = true;
        }

        private void Update()
        {
            if (_currentlyDraggedPart == null)
            {
                return;
            }

            Vector3 objectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            objectPosition = objectPosition.GridPosition();
            
            _currentlyDraggedPart.RootTransform.position = objectPosition;

            bool canBePlaced = ShipGridController.Current.CanBePlaced(objectPosition, _currentlyDraggedPart);
            
            _currentlyDraggedPart.ModuleColorController.ShowPositionValidity(canBePlaced);
            
            if (canBePlaced &&
                !startedDraggingThisFrame &&
                Input.GetMouseButtonDown(0))
            {
                ShipGridController.Current.PlacePart(objectPosition, _currentlyDraggedPart);
                _currentlyDraggedPart = null;
            }

            startedDraggingThisFrame = false;
        }
    }
}