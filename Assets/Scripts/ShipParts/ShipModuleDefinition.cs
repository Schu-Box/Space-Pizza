using UnityEngine;
using UnityEngine.Serialization;

namespace ShipParts
{
    [CreateAssetMenu(menuName = "Modules/ShipModuleDefinition")]
    public class ShipModuleDefinition: ScriptableObject
    {
        [SerializeField]
        private string displayName;

        public string DisplayName => displayName;

        [FormerlySerializedAs("shape")]
        [TextArea]
        [SerializeField] 
        private string shapeDefinition;

        public string ShapeDefinition => shapeDefinition;
    }
}