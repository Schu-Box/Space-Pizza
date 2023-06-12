using System;
using System.Collections.Generic;
using UnityEngine;
using ShipParts;

namespace Managers
{
    public class ConstructionManager: MonoBehaviour
    {
        public static ConstructionManager Current => GameManager.Instance.ReferenceProvider.ConstructionManager;
        
        [SerializeField] 
        private int constructionAreaWidth = 64;
        
        [SerializeField] 
        private int constructionAreaHeight = 64;

        [SerializeField] private int areaStartY = 0;

        [SerializeField] private int areaStartX = 0;

        private Dictionary<ShipPart, List<(int, int)>> occupiedSpacesByPart = new();

        private ShipPart[,] occupiedSpaces;

        private void Awake()
        {
            occupiedSpaces = new ShipPart[constructionAreaHeight,constructionAreaWidth];
        }

        public bool CanBePlaced(Vector3 position, ShipPart partToPlace)
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

            int[,] shape = partToPlace.Shape;
            
            for(int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] != 0 &&
                        occupiedSpaces[row + i, column + j] != null)
                    {
                        Debug.LogError($"Cannot be placed at ({row}, {column}) because shape part" +
                                       $" ({i}, {j}) is overlapping the occupied space" +
                                       $" ({row + i}, {column + j})");
                        
                        return false;
                    }
                }
            }

            return true;
        }

        private void ConvertPositionToIndices(Vector3 position, out int row, out int column)
        {
            column = Mathf.RoundToInt(position.x) - areaStartX;
            row = (Mathf.RoundToInt(position.y) - areaStartY) * -1;
        }

        public void PlacePart(Vector3 partPosition, ShipPart placedPart)
        {
            ConvertPositionToIndices(partPosition, out int row, out int column);

            int[,] shape = placedPart.Shape;
            
            for(int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] != 0)
                    {
                        Debug.LogError($"Shape of {placedPart.name} has an element at" +
                                       $" ({i}, {j})", placedPart);
                        occupiedSpaces[row + i, column + j] = placedPart;
                    }
                }
            }

            PrintArray();
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