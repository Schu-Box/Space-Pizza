using System.Collections;
using UnityEngine;

namespace ShipParts
{
    public class ModuleMover: MonoBehaviour
    {
        [SerializeField] 
        private ShipModule _shipPart;

        private Coroutine movementCoroutine;

        public void StartMoving(Vector3 startPosition, Vector3 targetPosition, float moveDuration)
        {
            if (movementCoroutine != null)
            {
                StopCoroutine(movementCoroutine);
            }

            movementCoroutine = StartCoroutine(Move(startPosition, targetPosition, moveDuration));
        }

        private IEnumerator Move(Vector3 startPosition, Vector3 targetPosition, float moveDuration)
        {
            float startTime = Time.time;
            
            _shipPart.RootTransform.position = startPosition;

            while (Vector3.Distance(_shipPart.RootTransform.position, targetPosition) > 0.1f)
            {
                float progress = (Time.time - startTime) / moveDuration;

                Vector3 lerpedPosition = Vector3.Lerp(startPosition, targetPosition, progress);

                _shipPart.RootTransform.position = lerpedPosition;

                yield return null;
            }
            
            _shipPart.RootTransform.position = targetPosition;

            movementCoroutine = null;
        }
        
        public void StopMoving()
        {
            if (movementCoroutine == null)
            {
                return;
            }
            
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
    }
}