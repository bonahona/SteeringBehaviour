using UnityEngine;
using UnityEngine.AI;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "NavMeshBehaviour", menuName = "Steering/Nav Mesh")]
    public class NavMeshSteeringBehaviour : SteeringBehaviourBase
    {
        public override SteeringData UpdateBehaviour(SteeringAgent agent)
        {
            SteeringDataCache.Reset();

            if(!agent.ActiveTarget()) {
                return SteeringDataCache;
            }

            if (agent.RepathTimer < 0) {
                Repath(agent);
            }

            agent.RepathTimer -= Time.fixedTime;

            if((agent.CurrentNavMeshPathIndex == 0 || agent.CurrentNavMeshPathIndex == agent.NavMeshPath.corners.Length) || agent.NavMeshPath.status != NavMeshPathStatus.PathComplete) {
                return SteeringDataCache;
            }

            DebugDrawPath(agent);

            return SteeringDataCache;
        }

        private void Repath(SteeringAgent agent)
        {
            if (NavMesh.CalculatePath(agent.transform.position, agent.Target.transform.position, int.MaxValue, agent.NavMeshPath)) {
                agent.CurrentNavMeshPathIndex = 1;
            } else {
                agent.CurrentNavMeshPathIndex = 0;
            }

            agent.RepathTimer = 1f;
        }

        private void DebugDrawPath(SteeringAgent agent)
        {
            for (int i = 1; i < agent.NavMeshPath.corners.Length; i++) {
                if (agent.CurrentNavMeshPathIndex == i) {
                    Debug.DrawLine(agent.NavMeshPath.corners[i - 1], agent.NavMeshPath.corners[i], Color.green);
                } else {
                    Debug.DrawLine(agent.NavMeshPath.corners[i - 1], agent.NavMeshPath.corners[i], Color.yellow);
                }
            }
        }
    }
}