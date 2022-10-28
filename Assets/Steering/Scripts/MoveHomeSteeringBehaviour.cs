using Fyrvall.SteeringBehaviour.Data;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "MoveHomeBehaviour", menuName = "Steering/Move home")]
    public class MoveHomeSteeringBehaviour: SteeringBehaviourBase
    {
        [Range(0f, 10f)]
        public float DesiredDistance = 1f;

        [Range(0f, 5f)]
        public float Priority = 1f;

        public override SteeringData UpdateBehaviour(SteeringAgent agent)
        {
            SteeringDataCache.Reset();
            if(agent.ActiveTarget()) {
                return SteeringDataCache;
            }

            var delta = (agent.StartPosition - agent.transform.position);
            var distance = delta.magnitude;
            if (distance > DesiredDistance) {
                SteeringDataCache.MovementFromDirection(delta.normalized, 1f, Priority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, 1f, Priority);
            } else {
                var weight = distance / DesiredDistance;
                SteeringDataCache.MovementFromDirection(delta.normalized, 1f, weight * Priority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, 1f, weight * Priority);
            }

            return SteeringDataCache;
        }
    }
}