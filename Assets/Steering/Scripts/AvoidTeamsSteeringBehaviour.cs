using Fyrvall.SteeringBehaviour.Data;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "AvoidTeamBehaviour", menuName = "Steering/Avoid Team")]
    public class AvoidTeamsSteeringBehaviour : SteeringBehaviourBase
    {
        public float DesiredDistance = 1f;

        [Range(0f, 5f)]
        public float Priority = 1f;

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
                WorkCache.MovementFromDirection(-delta.normalized, weight * Priority);
                WorkCache.OrientationFromDirection(-delta.normalized, weight * Priority);
                SteeringDataCache.Apply(WorkCache);
            }

            return SteeringDataCache;
        }
    }
}