using UnityEngine;

namespace Helpers
{
    public static class MonoBehaviourExtension
    {
        public static string ExtendedName(this MonoBehaviour targetObject)
        {
            string nameWithHierarchy = "";

            Transform currentTransform = targetObject.transform;
            
            for (int i = 0; i < 5; i++)
            {
                nameWithHierarchy += $"{currentTransform.name}/";

                currentTransform = currentTransform.parent;

                if (currentTransform == null)
                {
                    break;
                }
            }

            nameWithHierarchy = nameWithHierarchy.Trim('/');

            return nameWithHierarchy;
        }
    }
}