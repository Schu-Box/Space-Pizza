using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class ShipManager: MonoBehaviour
    {
        public static ShipManager Current => GameManager.Instance.ReferenceProvider.ShipManager;
        
        public Ship PlayerShip { get; private set; }

        public void RegisterShip(Ship ship)
        {
            PlayerShip = ship;
        }
    }
}