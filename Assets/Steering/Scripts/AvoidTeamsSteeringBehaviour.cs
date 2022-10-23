using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "AvoidTeamBehaviour", menuName = "Steering/Avoid Team")]
    public class AvoidTeamsSteeringBehaviour : SteeringBehaviourBase
    {
        public float DesiredDistance = 1f;

        [Range(0f, 5f)]
        public float Priority = 1f;

        public override SteeringData UpdateBehaviour(SteeringAgent agent)
        {
            SteeringDataCache.Reset();

            foreach (var friendlyAgent in agent.FriendlyAgents) {
                var delta = friendlyAgent.transform.position - agent.transform.position;

                if (delta.magnitude > DesiredDistance) {
                    continue;
                }

                var weight = 1f - (delta.magnitude / DesiredDistance);
                SteeringDataCache.MovementFromDirection(-delta.normalized, 1f, weight * Priority);
                SteeringDataCache.Apply(SteeringDataCache);
            }

            return SteeringDataCache;
        }
    }
}