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

                var distance = Mathf.Max(delta.magnitude - (agent.Radius + friendlyAgent.Radius), 0f);
                if (distance > DesiredDistance) {
                    continue;
                }

                var weight = 1f - (distance / DesiredDistance);
                WorkCache.MovementFromDirection(-delta.normalized, 1f, weight * Priority);
                WorkCache.OrientationFromDirection(-delta.normalized, 1f, weight * Priority);
                SteeringDataCache.Apply(WorkCache);
            }

            return SteeringDataCache;
        }
    }
}