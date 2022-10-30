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

        public override SteeringData UpdateBehaviour(SteeringAgent agent, float deltaTime)
        {
            SteeringDataCache.Reset();
            if(agent.ActiveTarget()) {
                return SteeringDataCache;
            }

            var delta = (agent.StartPosition - agent.transform.position);
            var distance = delta.magnitude;
            if (distance > DesiredDistance) {
                SteeringDataCache.MovementFromDirection(delta.normalized, Priority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, Priority);
            } else {
                var weight = distance / DesiredDistance;
                SteeringDataCache.MovementFromDirection(delta.normalized, weight * Priority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, weight * Priority);
            }

            return SteeringDataCache;
        }
    }
}