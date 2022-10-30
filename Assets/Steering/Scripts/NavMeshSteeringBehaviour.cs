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

        private float ClosestDistanceSqr;
        private float FinishDistanceSqr;
        private float DesiredDistanceSqr;

        public override void StartBehaviour(SteeringAgent agent)
        {
            ClosestDistanceSqr = ClosestDistance * ClosestDistance;
            FinishDistanceSqr = FinishDistance * FinishDistance;
            DesiredDistanceSqr = DesiredDistance * DesiredDistance;

            agent.NavMeshSteeringData.RepathTimer = Random.Range(0, RepathTimer);
        }

        public override SteeringData UpdateBehaviour(SteeringAgent agent, float deltaTime)
        {
            SteeringDataCache.Reset();

            if(!agent.ActiveTarget()) {
                return SteeringDataCache;
            }

            if (agent.NavMeshSteeringData.RepathTimer < 0) {
                Repath(agent);
            }

            agent.NavMeshSteeringData.RepathTimer -= deltaTime;

            if(!agent.NavMeshSteeringData.CanUsePath()) {
                return SteeringDataCache;
            }

            PathToNextCorner(agent);

            return SteeringDataCache;
        }

        private void Repath(SteeringAgent agent)
        {
            agent.NavMeshSteeringData.Repath(agent.transform.position, agent.Target.transform.position);
            agent.NavMeshSteeringData.SetRepathTimer(RepathTimer);
        }

        private void PathToNextCorner(SteeringAgent agent)
        {
            var nextCorner = agent.NavMeshSteeringData.NavMeshPath.corners[agent.NavMeshSteeringData.NextNavMeshPathIndex];
            var delta = nextCorner - agent.transform.position;

            if (delta.sqrMagnitude < FinishDistanceSqr) {
                agent.NavMeshSteeringData.NextNavMeshPathIndex++;
            }

            var distanceLeft = agent.NavMeshSteeringData.GetTotalPathDistanceLeft(agent);

            if(distanceLeft < ClosestDistance) {
                return;
            } else if (distanceLeft < DesiredDistance) {
                var weight = (distanceLeft - ClosestDistance) / (DesiredDistance - ClosestDistance);
                SteeringDataCache.MovementFromDirection(delta.normalized, weight * Priority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, weight * Priority);
            } else {
                SteeringDataCache.MovementFromDirection(delta.normalized, Priority);
                SteeringDataCache.OrientationFromDirection(delta.normalized, Priority);
            }
        }

#if UNITY_EDITOR
        public override void DebugDraw(SteeringAgent agent)
        {
            if(agent.Target == null) {
                return;
            }

            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(agent.Target.transform.position, Vector3.up, ClosestDistance, 2f);

            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(agent.Target.transform.position, Vector3.up, DesiredDistance, 2f);

            if(!agent.NavMeshSteeringData.HasPath) {
                return;
            }

            var corners = agent.NavMeshSteeringData.NavMeshPath.corners;
            //var distanceRemaining = agent.NavMeshSteeringData.GetTotalPathDistanceLeft(agent);
            for(int i = 0; i < corners.Length -1; i ++) {
                Debug.DrawLine(corners[i], corners[i + 1], Color.yellow);
            }
        }
#endif
    }
}