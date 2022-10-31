using Fyrvall.SteeringBehaviour.Data;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "AvoidTeamBehaviour", menuName = "Steering/Avoid Team")]
    public class AvoidTeamsSteeringBehaviour : SteeringBehaviourBase
    {
        public float DesiredDistance = 1f;

        private float DesiredDistanceSquare;

        public override void StartBehaviour(SteeringAgent agent)
        {
            DesiredDistanceSquare = DesiredDistance * DesiredDistance;
        }

        public override SteeringData UpdateBehaviour(SteeringAgent agent, float deltaTime)
        {
            SteeringDataCache.Reset();

            foreach (var friendlyAgent in agent.FriendlyAgents) {
                var delta = friendlyAgent.transform.position - agent.transform.position;


                if (delta.sqrMagnitude > DesiredDistanceSquare) {
                    continue;
                }

                var distance = delta.magnitude;
                var weight = 1f - (distance / DesiredDistance);
                WorkCache.MovementFromDirection(-delta.normalized, weight * MovementPriority);
                WorkCache.OrientationFromDirection(-delta.normalized, weight * OrientationPriority);
                SteeringDataCache.Apply(WorkCache);
            }

            return SteeringDataCache;
        }
    }
}