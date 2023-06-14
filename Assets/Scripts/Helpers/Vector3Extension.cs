using UnityEngine;

namespace Helpers
{
    public static class Vector3Extension
    {
        public static Vector3 GridPosition(this Vector3 worldPosition)
        {
            Vector3 gridPosition;
            
            gridPosition.x = Mathf.RoundToInt(worldPosition.x);
            gridPosition.y = Mathf.RoundToInt(worldPosition.y);
            gridPosition.z = 0;
            
            return gridPosition;
        }
    }
}