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

        public override void UpdateBehaviour(SteeringAgent agent, SteeringData steeringData)
        {
            steeringData.Reset();
            if (agent.Target == null) {
                return;
            }

            var delta = (agent.Target.transform.position - agent.transform.position);
            var distance = delta.magnitude;
            if (distance < ClosestDistance) {
                var weight = 1f - delta.magnitude / ClosestDistance;
                steeringData.MovementFromDirection(-delta.normalized, BackwardsFallof, weight * Priority * FromClosestDistancePriority);
                steeringData.OrientationFromDirection(delta.normalized, BackwardsFallof, weight * Priority * FromClosestDistancePriority);
            } else if (distance < DesiredDistance) {
                var weight = (delta.magnitude - ClosestDistance) / (DesiredDistance - ClosestDistance);
                steeringData.MovementFromDirection(delta.normalized, BackwardsFallof, weight * Priority);
                steeringData.OrientationFromDirection(delta.normalized, BackwardsFallof, weight * Priority);
            } else {
                steeringData.MovementFromDirection(delta.normalized, BackwardsFallof, Priority * ToDesiredDistancePriority);
                steeringData.OrientationFromDirection(delta.normalized, BackwardsFallof, Priority * ToDesiredDistancePriority);
            }
        }
    }
}