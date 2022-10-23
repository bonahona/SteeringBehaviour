using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "MoveHomeBehaviour", menuName = "Steering/Move home")]
    public class MoveHomeSteeringBehaviour: SteeringBehaviourBase
    {
        [Range(0f, 5f)]
        public float Priority = 1f;

        public override SteeringData UpdateBehaviour(SteeringAgent agent)
        {
            SteeringDataCache.Reset();
            if(agent.ActiveTarget()) {
                return SteeringDataCache;
            }

            var delta = (agent.StartPosition - agent.transform.position);
            SteeringDataCache.MovementFromDirection(-delta.normalized * Priority, 1f);
            return SteeringDataCache;
        }
    }
}