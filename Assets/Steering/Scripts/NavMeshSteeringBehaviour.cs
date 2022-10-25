using UnityEngine;
using UnityEngine.AI;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "NavMeshBehaviour", menuName = "Steering/Nav Mesh")]
    public class NavMeshSteeringBehaviour : SteeringBehaviourBase
    {
        public float ClosestDistance = 1f;
        public float DesiredDistance = 10f;
        public float RepathTimer = 2f;
        public float FinishDistance = 0.1f;
        public float Priority = 1f;

        [Header("Debug")]
        public bool DebugPath = false;

        public override void StartBehaviour(SteeringAgent agent)
        {
            agent.RepathTimer = Random.Range(0, RepathTimer);
        }

        public override SteeringData UpdateBehaviour(SteeringAgent agent)
        {
            SteeringDataCache.Reset();

            if(!agent.ActiveTarget()) {
                return SteeringDataCache;
            }

            if (agent.RepathTimer < 0) {
                Repath(agent);
            }

            agent.RepathTimer -= Time.fixedDeltaTime;

            if((agent.CurrentNavMeshPathIndex == 0 || agent.CurrentNavMeshPathIndex == agent.NavMeshPath.corners.Length) || agent.NavMeshPath.status != NavMeshPathStatus.PathComplete) {
                return SteeringDataCache;
            }

            PathToNextCorner(agent);
            DebugDrawPath(agent);

            return SteeringDataCache;
        }

        private void Repath(SteeringAgent agent)
        {
            var distanceToTarget = agent.Target.transform.position - agent.transform.position;
            if(distanceToTarget.magnitude < DesiredDistance) {
                agent.RepathTimer = RepathTimer;
                return;
            }

            if (NavMesh.CalculatePath(agent.transform.position, agent.Target.transform.position, int.MaxValue, agent.NavMeshPath)) {
                agent.CurrentNavMeshPathIndex = 1;
            } else {
                agent.CurrentNavMeshPathIndex = 0;
            }

            agent.RepathTimer = RepathTimer;
        }

        private void DebugDrawPath(SteeringAgent agent)
        {
            if(!DebugPath) {
                return;
            }

            for (int i = 1; i < agent.NavMeshPath.corners.Length; i++) {
                if (agent.CurrentNavMeshPathIndex == i) {
                    Debug.DrawLine(agent.NavMeshPath.corners[i - 1], agent.NavMeshPath.corners[i], Color.green);
                } else {
                    Debug.DrawLine(agent.NavMeshPath.corners[i - 1], agent.NavMeshPath.corners[i], Color.yellow);
                }
            }
        }

        private void PathToNextCorner(SteeringAgent agent)
        {
            var nextCorner = agent.NavMeshPath.corners[agent.CurrentNavMeshPathIndex];
            var delta = nextCorner - agent.transform.position;

            var distance = delta.magnitude;
            if (distance < FinishDistance) {
                agent.CurrentNavMeshPathIndex++;
            }

            var totalDistance = Mathf.Max(TotalPathDistance(agent) - agent.Radius);
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

        private float TotalPathDistance(SteeringAgent agent)
        {
            var result = 0f;

            if(agent.NavMeshPath.status != NavMeshPathStatus.PathComplete) {
                return result;
            }

            for (int i = 1; i < agent.NavMeshPath.corners.Length; i++) {
                if(agent.CurrentNavMeshPathIndex == i) {
                    result += (agent.transform.position - agent.NavMeshPath.corners[i]).magnitude;
                } else {
                    result += (agent.NavMeshPath.corners[i - 1] - agent.NavMeshPath.corners[i]).magnitude;
                }
            }

            return result;
        }
    }
}