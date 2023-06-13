using UnityEngine;
using UnityEngine.Serialization;

namespace ShipParts
{
    [CreateAssetMenu(menuName = "Modules/ShipModuleDefinition")]
    public class ShipModuleDefinition: ScriptableObject
    {
        [FormerlySerializedAs("shape")]
        [TextArea]
        [SerializeField] 
        private string shapeDefinition;

        public string ShapeDefinition => shapeDefinition;
    }
}