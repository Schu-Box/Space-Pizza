using System;
using ShipParts;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class DragAndDropManager: MonoBehaviour
    {
        public static DragAndDropManager Current => GameManager.Instance.ReferenceProvider.DragAndDropManager;
        
        private ShipPart _currentlyDraggedPart = null;

        private bool startedDraggingThisFrame = false;

        public void StartDragging(ShipPart objectToDrag)
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

            objectPosition.x = Mathf.RoundToInt(objectPosition.x);
            objectPosition.y = Mathf.RoundToInt(objectPosition.y);
            objectPosition.z = 0;
            
            _currentlyDraggedPart.RootTransform.position = objectPosition;

            bool canBePlaced = ConstructionManager.Current.CanBePlaced(objectPosition, _currentlyDraggedPart);
            
            _currentlyDraggedPart.PlacementVisualizer.ChangeColor(canBePlaced);
            
            if (canBePlaced &&
                !startedDraggingThisFrame &&
                Input.GetMouseButtonDown(0))
            {
                ConstructionManager.Current.PlacePart(objectPosition, _currentlyDraggedPart);
                _currentlyDraggedPart = null;
            }

            startedDraggingThisFrame = false;
        }
    }
}