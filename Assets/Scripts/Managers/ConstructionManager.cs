using System;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using ShipParts;

namespace Managers
{
    public class ConstructionManager: MonoBehaviour
    {
        public static ConstructionManager Current => GameManager.Instance.ReferenceProvider.ConstructionManager;

        [SerializeField] 
        private GameObject shipCorePrefab;

        [SerializeField] 
        private Vector3 corePosition;
        
        [SerializeField] 
        private int constructionAreaWidth = 64;
        
        [SerializeField] 
        private int constructionAreaHeight = 64;
        
        [SerializeField] private int areaStartX = 0;
        
        [SerializeField] private int areaStartY = 0;

        private Dictionary<ShipModule, List<(int, int)>> occupiedSpacesByPart = new();

        private ShipModule[,] occupiedSpaces;

        private List<(int, int)> validNeighborPositions = new List<(int, int)>
        {
            (-1, 0),
            (0, -1),
            (0, 1),
            (1, 0)
        };

        private void Start()
        {
            occupiedSpaces = new ShipModule[constructionAreaHeight,constructionAreaWidth];

            Vector3 coreGridPosition = corePosition.GridPosition();
            
            GameObject shipCore = Instantiate(shipCorePrefab, coreGridPosition, Quaternion.identity);

            ShipModule coreLogic = shipCore.GetComponentInChildren<ShipModule>();

            if (coreLogic == null)
            {
                Debug.LogError($"[ConstructionManager] The ship core prefab is missing a ship" +
                               $" part component!");
            }
            
            PlacePart(coreGridPosition, coreLogic);
        }

        public bool CanBePlaced(Vector3 position, ShipModule partToPlace)
        {
            ConvertPositionToIndices(position, out int row, out int column);

            if (column < 0 || column >= constructionAreaWidth)
            {
                return false;
            }

            if (row < 0 || row >= constructionAreaHeight)
            {
                return false;
            }

            bool isShapeConnectedToExistingPart = false;
            
            List<(int, int)> shapeInformation = partToPlace.ShapeAsList;

            foreach ((int, int) takenPosition in shapeInformation)
            {
                int shapeRow = row + takenPosition.Item1;
                int shapeColumn = column + takenPosition.Item2;
                
                if (occupiedSpaces[shapeRow, shapeColumn] != null)
                {
                    return false;
                }

                if (isShapeConnectedToExistingPart)
                {
                    continue;
                }

                foreach ((int, int) validNeighborPosition in validNeighborPositions)
                {
                    int neighborRow = shapeRow + validNeighborPosition.Item1;
                    int neighborColumn = shapeColumn + validNeighborPosition.Item2;

                    if (neighborRow < 0 || neighborRow >= constructionAreaHeight)
                    {
                        continue;
                    }
                        
                    if (neighborColumn < 0 || neighborColumn >= constructionAreaWidth)
                    {
                        continue;
                    }

                    if (occupiedSpaces[neighborRow, neighborColumn] != null)
                    {
                        // there is an existing part of the ship next to this part of the shape :)
                        isShapeConnectedToExistingPart = true;
                        break;
                    }
                }
            }
            
            // for(int i = 0; i < shape.GetLength(0); i++)
            // {
            //     for (int j = 0; j < shape.GetLength(1); j++)
            //     {
            //         if (shape[i, j] != 0 &&
            //             occupiedSpaces[row + i, column + j] != null)
            //         {
            //             Debug.LogError($"Cannot be placed at ({row}, {column}) because shape part" +
            //                            $" ({i}, {j}) is overlapping the occupied space" +
            //                            $" ({row + i}, {column + j})");
            //             
            //             return false;
            //         }
            //     }
            // }

            return isShapeConnectedToExistingPart;
        }

        private void ConvertPositionToIndices(Vector3 position, out int row, out int column)
        {
            column = Mathf.RoundToInt(position.x) - areaStartX;
            row = (Mathf.RoundToInt(position.y) - areaStartY) * -1;
        }

        public void PlacePart(Vector3 partPosition, ShipModule placedModule)
        {
            ConvertPositionToIndices(partPosition, out int row, out int column);

            int[,] shape = placedModule.Shape;
            
            for(int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] != 0)
                    {
                        Debug.LogError($"Shape of {placedModule.name} has an element at" +
                                       $" ({i}, {j})", placedModule);
                        occupiedSpaces[row + i, column + j] = placedModule;
                    }
                }
            }

            ShipManager.Current.PlayerShip.AddModule(placedModule);
            // PrintArray();
        }

        private void PrintArray()
        {
            for (int i = 0; i < constructionAreaHeight; i++)
            {
                string rowString = "";
                
                for (int j = 0; j < constructionAreaWidth; j++)
                {
                    if (occupiedSpaces[i, j] != null)
                    {
                        rowString += "x";
                    }
                    else
                    {
                        rowString += "O";
                    }
                }
                
                Debug.LogError($"Row {i}: {rowString}");
            }
        }
    }
}