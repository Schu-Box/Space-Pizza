using System;
using System.Collections.Generic;
using GamePhases;
using Helpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class ShipGridController : MonoBehaviour
    {
        public static ShipGridController Current => GameManager.Instance.ReferenceProvider.ShipGridController;

        [SerializeField]
        private GameObject shipCorePrefab;

        [SerializeField]
        private Vector3 corePosition;
        public Vector3 CorePosition => corePosition;

        [FormerlySerializedAs("constructionAreaWidth")]
        [SerializeField]
        private int shipGridWidth = 64;

        [FormerlySerializedAs("constructionAreaHeight")]
        [SerializeField]
        private int shipGridHeight = 64;

        [SerializeField]
        private int areaStartX = 0;

        [SerializeField]
        private int areaStartY = 0;

        [FormerlySerializedAs("placeSfxPlayer")]
        [SerializeField]
        private RandomizedAudioPlayer placeModuleSfxPlayer;

        private Dictionary<ShipModule, List<(int, int)>> occupiedSpacesByPart = new();

        private ShipModule[,] shipGrid;

        private List<(int, int)> validNeighborPositions = new List<(int, int)>
        {
            (-1, 0),
            (0, -1),
            (0, 1),
            (1, 0)
        };

        private void Awake()
        {
            shipGrid = new ShipModule[shipGridHeight, shipGridWidth];
        }

        private void Start()
        {
            PhaseManager.Current.PhaseChangedEvent += SpawnStartingShip;
            
            SpawnStartingShip();
        }

        private void SpawnStartingShip()
        {
            if (PhaseManager.Current.CurrentPhase != GamePhase.Construction || ShipManager.Current.PlayerShip != null)
            {
                return;
            }

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

            if (column < 0 || column >= shipGridWidth)
            {
                return false;
            }

            if (row < 0 || row >= shipGridHeight)
            {
                return false;
            }

            bool isShapeConnectedToExistingPart = false;

            List<(int, int)> shapeInformation = partToPlace.ShapeAsList;

            foreach ((int, int) takenPosition in shapeInformation)
            {
                int shapeRow = row + takenPosition.Item1;
                int shapeColumn = column + takenPosition.Item2;

                if (shipGrid[shapeRow, shapeColumn] != null)
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

                    if (neighborRow < 0 || neighborRow >= shipGridHeight)
                    {
                        continue;
                    }

                    if (neighborColumn < 0 || neighborColumn >= shipGridWidth)
                    {
                        continue;
                    }

                    if (shipGrid[neighborRow, neighborColumn] != null)
                    {
                        // there is an existing part of the ship next to this part of the shape :)
                        isShapeConnectedToExistingPart = true;
                        break;
                    }
                }
            }

            return isShapeConnectedToExistingPart;
        }

        private void ConvertPositionToIndices(Vector3 position, out int row, out int column)
        {
            column = Mathf.RoundToInt(position.x) - areaStartX;
            row = (Mathf.RoundToInt(position.y) - areaStartY) * -1;
        }

        public void PlacePart(Vector3 partPosition, ShipModule placedModule)
        {
            placeModuleSfxPlayer.Play();
            
            if (!placedModule.coreModule && 
                PhaseManager.Current.CurrentPhase == GamePhase.Construction && 
                ConstructionInterfaceManager.Instance.TimerStarted == false)
            {
                ConstructionInterfaceManager.Instance.StartTimer();
            }

            ConvertPositionToIndices(partPosition, out int row, out int column);

            // store list of which position in the grid the module is occupying for easy lookup
            List<(int, int)> partPositions = new();

            int[,] shape = placedModule.Shape;

            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] != 0)
                    {
                        int gridRow = row + i;
                        int gridColumn = column + j;
                        
                        partPositions.Add((gridRow, gridColumn));
                        shipGrid[gridRow, gridColumn] = placedModule;
                    }
                }
            }

            occupiedSpacesByPart[placedModule] = partPositions;

            // if (!ShipManager.Current.PlayerShip.HasCore)
            // {
                ShipManager.Current.PlayerShip.AddModule(placedModule);
            // }
        }

        public List<ShipModule> FindNeighbors(ShipModule shipModule)
        {
            if (!occupiedSpacesByPart.TryGetValue(shipModule, out List<(int, int)> occupiedSpaces))
            {
                Debug.LogError($"[ShipGridController] FindNeighbors called with {shipModule.name}" +
                               $" which does not seem to be part of the ship!");
                return new();
            }

            List<ShipModule> neighbors = new();

            foreach ((int, int) gridCoordinate in occupiedSpaces)
            {
                foreach ((int, int) validNeighborPosition in validNeighborPositions)
                {
                    int neighbourRow = gridCoordinate.Item1 + validNeighborPosition.Item1;
                    int neighbourColumn = gridCoordinate.Item2 + validNeighborPosition.Item2;

                    if (neighbourRow < 0 || neighbourRow >= shipGridHeight)
                    {
                        continue;
                    }
                    
                    if (neighbourColumn < 0 || neighbourColumn >= shipGridWidth)
                    {
                        continue;
                    }

                    ShipModule possibleNeighbor = shipGrid[neighbourRow, neighbourColumn];

                    if (possibleNeighbor != null &&
                        possibleNeighbor != shipModule &&
                        !neighbors.Contains(possibleNeighbor))
                    {
                        neighbors.Add(possibleNeighbor);
                    }
                }
            }

            return neighbors;
        }

        private void OnDestroy()
        {
            PhaseManager.Current.PhaseChangedEvent -= SpawnStartingShip;
        }
    }
}