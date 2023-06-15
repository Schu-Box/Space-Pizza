using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShipParts
{
    public class ModuleColorController: MonoBehaviour
    {
        [SerializeField]
        private Color invalidPlacementColor = Color.red;
        
        [SerializeField]
        private Color moduleDamagedColor = Color.gray;

        [SerializeField]
        private ShipModule _shipModule;
        
        [SerializeField] 
        private List<SpriteRenderer> renderersToColor = new();

        private List<Color> defaultColors = new();

        private void Awake()
        {
            foreach (SpriteRenderer spriteRenderer in renderersToColor)
            {
                defaultColors.Add(spriteRenderer.color);
            }

            _shipModule.HealthChangedEvent += UpdateHealthRelatedColoring;
        }

        private void OnDestroy()
        {
            _shipModule.HealthChangedEvent -= UpdateHealthRelatedColoring;
        }

        private void UpdateHealthRelatedColoring()
        {
            if (_shipModule.CurrentHealth >= _shipModule.MaxHealth)
            {
                ResetColor();
                return;
            }
            
            ShowDamage();
        }

        public void ShowPositionValidity(bool isPlacementValid)
        {
            if (isPlacementValid)
            {
                ResetColor();
                return;
            }

            ShowInvalidPosition();
        }
        
        public void ShowInvalidPosition()
        {
            ChangeColor(invalidPlacementColor);
        }

        public void ResetColor()
        {
            for (int i = 0; i < renderersToColor.Count; i++)
            {
                renderersToColor[i].color = defaultColors[i];
            }
        }

        public void ShowDamage()
        {
            ChangeColor(moduleDamagedColor);
        }

        private void ChangeColor(Color targetColor)
        {
            foreach (SpriteRenderer spriteRenderer in renderersToColor)
            {
                spriteRenderer.color = targetColor;
            }
        }
    }
}