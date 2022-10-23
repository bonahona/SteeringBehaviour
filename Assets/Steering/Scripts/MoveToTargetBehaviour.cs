using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "MoveToTargetBehaviour", menuName = "Steering/Move to Target")]
    public class MoveToTargetBehaviour: SteeringBehaviourBase
    {
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

        public override SteeringData UpdateBehaviour(SteeringAgent agent)
        {
            SteeringDataCache.Reset();
            if (!agent.ActiveTarget()) {
                return SteeringDataCache;
            }

            var delta = (agent.Target.transform.position - agent.transform.position);
            var distance = delta.magnitude;
            if (distance < ClosestDistance) {
                var weight = 1f - delta.magnitude / ClosestDistance;
                SteeringDataCache.MovementFromDirection(-delta.normalized, BackwardsFallof, weight * Priority * FromClosestDistancePriority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, BackwardsFallof, Priority * FromClosestDistancePriority);
            } else if (distance < DesiredDistance) {
                var weight = (delta.magnitude - ClosestDistance) / (DesiredDistance - ClosestDistance);
                SteeringDataCache.MovementFromDirection(delta.normalized, BackwardsFallof, weight * Priority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, BackwardsFallof, Priority);
            } else {
                SteeringDataCache.MovementFromDirection(delta.normalized, BackwardsFallof, Priority * ToDesiredDistancePriority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, BackwardsFallof, Priority * ToDesiredDistancePriority);
            }

            return SteeringDataCache;
        }
    }
}