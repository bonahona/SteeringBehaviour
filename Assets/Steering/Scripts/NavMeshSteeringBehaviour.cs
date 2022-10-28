using Fyrvall.SteeringBehaviour.Data;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "NavMeshBehaviour", menuName = "Steering/Nav Mesh")]
    public class NavMeshSteeringBehaviour : SteeringBehaviourBase
    {
        public float ClosestDistance = 1f;
        public float DesiredDistance = 10f;
        public float RepathTimer = 1f;
        public float FinishDistance = 0.1f;
        public float Priority = 1f;

        public override void StartBehaviour(SteeringAgent agent)
        {
            agent.NavMeshSteeringData.RepathTimer = Random.Range(0, RepathTimer);
        }

        public override SteeringData UpdateBehaviour(SteeringAgent agent)
        {
            SteeringDataCache.Reset();

            if(!agent.ActiveTarget()) {
                return SteeringDataCache;
            }

            if (agent.NavMeshSteeringData.RepathTimer < 0) {
                Repath(agent);
            }

            agent.NavMeshSteeringData.RepathTimer -= Time.fixedDeltaTime;

            if(!agent.NavMeshSteeringData.CanUsePath()) {
                return SteeringDataCache;
            }

            PathToNextCorner(agent);

            return SteeringDataCache;
        }

        private void Repath(SteeringAgent agent)
        {
            var distanceToTarget = agent.Target.transform.position - agent.transform.position;
            if(distanceToTarget.magnitude < DesiredDistance) {
                agent.NavMeshSteeringData.SetRepathTimer(RepathTimer);
                return;
            }

            agent.NavMeshSteeringData.Repath(agent.transform.position, agent.Target.transform.position);

            agent.NavMeshSteeringData.SetRepathTimer(RepathTimer);
        }

        private void PathToNextCorner(SteeringAgent agent)
        {
            var nextCorner = agent.NavMeshSteeringData.NavMeshPath.corners[agent.NavMeshSteeringData.CurrentNavMeshPathIndex];
            var delta = nextCorner - agent.transform.position;

            var distance = delta.magnitude;
            if (distance < FinishDistance) {
                agent.NavMeshSteeringData.CurrentNavMeshPathIndex++;
            }

            var totalDistance = agent.NavMeshSteeringData.GetTotalPathDistance(agent);
            if(totalDistance < ClosestDistance) {
                return;
            } else if (totalDistance < DesiredDistance) {
                var weight = totalDistance / (DesiredDistance - ClosestDistance);
                SteeringDataCache.MovementFromDirection(delta.normalized, 0f, weight * Priority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, 0f, weight * Priority);
            } else {
                SteeringDataCache.MovementFromDirection(delta.normalized, 0f, Priority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, 0f, Priority);
            }
        }
    }
}