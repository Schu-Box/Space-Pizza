using System;
using Helpers;
using ShipParts;
using UnityEngine;

namespace Managers
{
    public class DragAndDropManager: MonoBehaviour
    {
        public static DragAndDropManager Current => GameManager.Instance.ReferenceProvider.DragAndDropManager;

        public event Action<ShipModuleDefinition> grabbedModuleChangedEvent;
        
        private ShipModule _currentlyDraggedPart = null;
        public ShipModule CurrentlyDraggedPart => _currentlyDraggedPart;

        private bool startedDraggingThisFrame = false;

        public void StartDragging(ShipModule objectToDrag)
        {
            if (_currentlyDraggedPart == objectToDrag)
            {
                return;
            }
            
            if (_currentlyDraggedPart != null)
            {
                Debug.LogError("Already dragging something!");
                return;
            }

            _currentlyDraggedPart = objectToDrag;
            startedDraggingThisFrame = true;
            
            grabbedModuleChangedEvent?.Invoke(_currentlyDraggedPart.ModuleDefinition);
            _currentlyDraggedPart.HandleModuleGrabbed();
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

            bool doesPlayerTryToPlace = Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0);
            
            if (doesPlayerTryToPlace &&
                canBePlaced &&
                !startedDraggingThisFrame)
            {
                ShipGridController.Current.PlacePart(objectPosition, _currentlyDraggedPart);
                StopDragging(_currentlyDraggedPart);
            }

            startedDraggingThisFrame = false;
        }

        public void StopDragging(ShipModule shipModule)
        {
            if (_currentlyDraggedPart != shipModule)
            {
                return;
            }
            
            _currentlyDraggedPart.ModuleColorController.ResetColor();
            _currentlyDraggedPart = null;
            
            grabbedModuleChangedEvent?.Invoke(null);
        }
    }
}