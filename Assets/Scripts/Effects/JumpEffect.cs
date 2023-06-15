using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Effects
{
    public class JumpEffect: MonoBehaviour
    {
        [SerializeField]
        private Volume renderVolume;

        [SerializeField]
        private AnimationCurve chromaticAberrationIntensity;
        
        [SerializeField]
        private AnimationCurve lensDistortionIntensity;

        [SerializeField]
        private float shipJumpPoint;

        [ContextMenu("Test effect")]
        public void TestEffect()
        {
            StartEffect(5f);
        }

        public void StartEffect(float effectDuration)
        {
            StartCoroutine(RunEffect(effectDuration));
        }

        private IEnumerator RunEffect(float effectDuration)
        {
            if (!renderVolume.profile.TryGet(out ChromaticAberration chromaticAberration))
            {
                yield break;
            }

            if (!renderVolume.profile.TryGet(out LensDistortion lensDistortion))
            {
                yield break;
            }
            
            float startTime = Time.time;

            Ship playerShip = ShipManager.Current.PlayerShip;

            bool disabledShip = false;
            
            while (Time.time - startTime <= effectDuration)
            {
                float progress = (Time.time - startTime) / effectDuration;

                if (progress >= shipJumpPoint &&
                    !disabledShip)
                {
                    playerShip.ChangeVisibility(false);
                    disabledShip = true;
                }
                
                chromaticAberration.intensity.value = chromaticAberrationIntensity.Evaluate(progress);

                lensDistortion.intensity.value = lensDistortionIntensity.Evaluate(progress);
                yield return null;
            }
        }
    }
}