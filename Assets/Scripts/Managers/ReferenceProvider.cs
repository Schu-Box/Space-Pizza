using GamePhases;
using UnityEngine;

namespace Managers
{
    public class ReferenceProvider: MonoBehaviour
    {
        [SerializeField] private DragAndDropManager _dragAndDropManager;

        public DragAndDropManager DragAndDropManager => _dragAndDropManager;

        [SerializeField] private PhaseManager phaseManager;

        public PhaseManager PhaseManager => phaseManager;

        [SerializeField] private ConstructionManager constructionManager;

        public ConstructionManager ConstructionManager => constructionManager;
    }
}