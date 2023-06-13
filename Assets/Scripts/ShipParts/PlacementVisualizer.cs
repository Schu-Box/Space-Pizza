using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShipParts
{
    public class PlacementVisualizer: MonoBehaviour
    {
        [SerializeField]
        private Color invalidPlacementColor = Color.red;
        
        [SerializeField] 
        private List<SpriteRenderer> renderersToColor = new();

        private List<Color> defaultColors = new();

        private void Awake()
        {
            foreach (SpriteRenderer spriteRenderer in renderersToColor)
            {
                defaultColors.Add(spriteRenderer.color);
            }
        }

        public void ChangeColor(bool isPlacementValid)
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
            foreach (SpriteRenderer spriteRenderer in renderersToColor)
            {
                spriteRenderer.color = invalidPlacementColor;
            }
        }

        public void ResetColor()
        {
            for (int i = 0; i < renderersToColor.Count; i++)
            {
                renderersToColor[i].color = defaultColors[i];
            }
        }
    }
}