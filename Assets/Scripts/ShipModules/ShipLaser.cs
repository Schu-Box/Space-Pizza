using UnityEngine;
[System.Serializable]  [CreateAssetMenu(fileName = "ShipLaser", menuName = "ShipLaser", order = 1)]
public class ShipLaser : ShipSubModule
{
    public void FireLaser()
    {
        Debug.Log("firing");
    }
}
