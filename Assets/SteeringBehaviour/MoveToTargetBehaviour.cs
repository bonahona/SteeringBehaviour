using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class MoveToTargetBehaviour : MonoBehaviour, ISteeringBehaviour
    {
        public Transform Target;
        public float ClosestDistance = 1f;
        public float DesiredDistance = 1f;

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
                SteeringData.FromDirection(-delta.normalized, BackwardsFallof, weight);
            } else if (distance < DesiredDistance) {
                var weight = (delta.magnitude - ClosestDistance) / (DesiredDistance - ClosestDistance);
                SteeringData.FromDirection(delta.normalized, BackwardsFallof, weight);
            } else {
                SteeringData.FromDirection(delta.normalized * Priority, BackwardsFallof);
            }
        }

        public SteeringData GetSteeringData() => SteeringData;
    }
}