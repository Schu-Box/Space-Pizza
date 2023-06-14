using GamePhases;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class ReferenceProvider: MonoBehaviour
    {
        [SerializeField] private DragAndDropManager _dragAndDropManager;

        public DragAndDropManager DragAndDropManager => _dragAndDropManager;

        [SerializeField] private PhaseManager phaseManager;

        public PhaseManager PhaseManager => phaseManager;

        [FormerlySerializedAs("constructionManager")]
        [SerializeField] private ShipGridController shipGridController;

        public ShipGridController ShipGridController => shipGridController;

        [SerializeField]
        private ShipManager shipManager;
        public ShipManager ShipManager => shipManager;
        
        [SerializeField]
        private InputManager inputManager;
        public InputManager InputManager => inputManager;
    }
}