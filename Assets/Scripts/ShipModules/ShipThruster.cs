using UnityEngine;

[System.Serializable] [CreateAssetMenu(fileName = "ShipThruster", menuName = "ShipThruster", order = 0)]
public class ShipThruster : ShipSubModule
{
    public float speed = 2f;
    public float rotationSpeed = 20f;
}
