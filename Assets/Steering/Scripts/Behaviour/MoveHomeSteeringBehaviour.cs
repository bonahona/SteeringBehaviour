using Fyrvall.SteeringBehaviour.Data;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "MoveHomeBehaviour", menuName = "Steering/Move home")]
    public class MoveHomeSteeringBehaviour: SteeringBehaviourBase
    {
        [Range(0f, 10f)]
        public float DesiredDistance = 1f;

        public override SteeringData UpdateBehaviour(SteeringAgent agent, float deltaTime)
        {
            SteeringDataCache.Reset();
            if(agent.ActiveTarget()) {
                return SteeringDataCache;
            }

            var delta = (agent.StartPosition - agent.transform.position);
            var distance = delta.magnitude;
            if (distance > DesiredDistance) {
                SteeringDataCache.MovementFromDirection(delta.normalized, MovementPriority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, OrientationPriority);
            } else {
                var weight = distance / DesiredDistance;
                SteeringDataCache.MovementFromDirection(delta.normalized, weight * MovementPriority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, weight * OrientationPriority);
            }

            return SteeringDataCache;
        }
    }
}