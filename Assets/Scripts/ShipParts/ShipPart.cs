using System;
using UnityEngine;

namespace ShipParts
{
    public class ShipPart: MonoBehaviour
    {
        [SerializeField] private Transform rootTransform;
        
        public Transform RootTransform => rootTransform;
        
        [SerializeField] private PlacementVisualizer placementVisualizer;

        public PlacementVisualizer PlacementVisualizer => placementVisualizer;

        [SerializeField] private ShipModuleDefinition _moduleDefinition;

        private int[,] shape = new int[5,5];

        public int[,] Shape => shape;

        private void Awake()
        {
            if (_moduleDefinition == null)
            {
                Debug.LogError($"[ShipPart] is missing its module definition!", this);
            }

            string shapeDefinition = _moduleDefinition.ShapeDefinition;

            string[] rows = shapeDefinition.Split(',');

            for (int i = 0; i < rows.Length; i++)
            {
                string row = rows[i];
                
                Debug.LogError(row);

                int parsedValidSymbols = 0;

                for (int j = 0; j < row.Length; j++)
                { 
                    char symbol = row[j];

                    if (symbol != '0' && symbol != '1')
                    {
                        continue;
                    }
                    
                    Debug.LogError($"symbol at ({i}, {parsedValidSymbols}): {symbol}");

                    if(symbol == '1')
                    {
                        Shape[i, parsedValidSymbols] = 1;
                    }

                    parsedValidSymbols += 1;
                }
            }

            for (int i = 0; i < shape.GetLength(0); i++)
            {
                string rowString = "";
                
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    rowString += shape[i, j];
                }
                
                Debug.LogError($"Shape at row {i}: {rowString}");
            }
        }
    }
}