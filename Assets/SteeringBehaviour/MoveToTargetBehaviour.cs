using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class MoveToTargetBehaviour : MonoBehaviour, SteeringBehaviourBase
    {
        public Transform Target;
        public float ClosestDistance = 1f;
        public float DesiredDistance = 1f;

        [Range(0, 5f)]
        public float ToDesiredDistancePriority = 1f;

        [Range(0, 5f)]
        public float FromClosestDistancePriority = 1f;

        [Range(0f, 1f)]
        public float BackwardsFallof = 0f;

        [Range (0f, 5f)]
        public float Priority = 1f;

        [HideInInspector]
        public SteeringData SteeringData = new SteeringData();

        public void UpdateBehaviour()
        {
            SteeringData.Reset();
            if (Target == null) {
                return;
            }

            var delta = (Target.position - transform.position);
            var distance = delta.magnitude;
            if (distance < ClosestDistance) {
                var weight = 1f - delta.magnitude / ClosestDistance;
                SteeringData.MovementFromDirection(-delta.normalized, BackwardsFallof, weight * Priority * FromClosestDistancePriority);
                SteeringData.OrientationFromDirection(delta.normalized, BackwardsFallof, weight * Priority * FromClosestDistancePriority);
            } else if (distance < DesiredDistance) {
                var weight = (delta.magnitude - ClosestDistance) / (DesiredDistance - ClosestDistance);
                SteeringData.MovementFromDirection(delta.normalized, BackwardsFallof, weight * Priority);
                SteeringData.OrientationFromDirection(delta.normalized, BackwardsFallof, weight * Priority);
            } else {
                SteeringData.MovementFromDirection(delta.normalized, BackwardsFallof, Priority * ToDesiredDistancePriority);
                SteeringData.OrientationFromDirection(delta.normalized, BackwardsFallof, Priority * ToDesiredDistancePriority);
            }
        }

        public SteeringData GetSteeringData() => SteeringData;
    }
}