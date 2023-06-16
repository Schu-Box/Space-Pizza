using UnityEngine;

namespace Helpers
{
    public static class AnimationCurveExtension
    {
        public static float EvaluateLimitless(this AnimationCurve animationCurve, float time)
        {
            animationCurve.postWrapMode = WrapMode.ClampForever;

            return animationCurve.Evaluate(time);
        }
    }
}