using System;
using Managers;
using TMPro;
using UnityEngine;

namespace ShipParts
{
    public class ModuleNameUi: MonoBehaviour
    {
        [SerializeField]
        private TMP_Text moduleNameField;

        private void Start()
        {
            DragAndDropManager.Current.grabbedModuleChangedEvent += UpdateNameField;
            UpdateNameField(null);
        }

        private void OnDestroy()
        {
            DragAndDropManager.Current.grabbedModuleChangedEvent -= UpdateNameField;
        }

        private void UpdateNameField(ShipModuleDefinition grabbedModuleDefinition)
        {
            string updatedText = "";
            
            if (grabbedModuleDefinition != null)
            {
                updatedText = grabbedModuleDefinition.DisplayName;
            }

            moduleNameField.text = updatedText;
        }
    }
}