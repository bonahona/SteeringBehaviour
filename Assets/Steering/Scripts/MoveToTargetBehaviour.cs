using Fyrvall.SteeringBehaviour.Data;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "MoveToTargetBehaviour", menuName = "Steering/Move to Target")]
    public class MoveToTargetBehaviour: SteeringBehaviourBase
    {
        public float ClosestDistance = 1f;
        public float DesiredDistance = 1f;
        public float FarthestDistance = 10f;

        [Range(0, 5f)]
        public float ToDesiredDistancePriority = 1f;

        [Range(0, 5f)]
        public float FromClosestDistancePriority = 1f;

        [Range(0f, 1f)]
        public float BackwardsFallof = 0f;

        public bool UseRaycast = true;
        public LayerMask GroundLayer;

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

            if (UseRaycast && !LineOfSightToTarget(agent, delta, distance)) {
                return SteeringDataCache;
            }

            if (distance < ClosestDistance) {
                var weight = 1f - distance / ClosestDistance;
                SteeringDataCache.MovementFromDirection(-delta.normalized, BackwardsFallof, weight * Priority * FromClosestDistancePriority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, BackwardsFallof, Priority * FromClosestDistancePriority);
            } else if (distance < DesiredDistance) {
                var weight = (distance - ClosestDistance) / (DesiredDistance - ClosestDistance);
                SteeringDataCache.MovementFromDirection(delta.normalized, BackwardsFallof, weight * Priority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, BackwardsFallof, Priority);
            } else if(distance < FarthestDistance){
                SteeringDataCache.MovementFromDirection(delta.normalized, BackwardsFallof, Priority * ToDesiredDistancePriority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, BackwardsFallof, Priority * ToDesiredDistancePriority);
            } 

            return SteeringDataCache;
        }

        private bool LineOfSightToTarget(SteeringAgent agent, Vector3 delta, float distance)
        {
            return !Physics.Raycast(agent.transform.position, delta, distance, GroundLayer.value);
        }
    }
}