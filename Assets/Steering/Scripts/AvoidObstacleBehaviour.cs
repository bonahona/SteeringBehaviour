using System.Collections.Generic;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "Avoid Obstacle", menuName = "Steering/Avoid Obstacle")]
    public class AvoidObstacleBehaviour : SteeringBehaviourBase
    {
        protected static List<SteeringObstacle> AllSteeringObstacle = new List<SteeringObstacle>();

        [Range(0f, 50f)]
        public float LookAheadDistance = 10f;
        [Range(0f, 5f)]
        public float Priority = 1f;

        public static void RegisterObstacle(SteeringObstacle steeringObstacle)
        {
            AllSteeringObstacle.Add(steeringObstacle);
        }

        public override SteeringData UpdateBehaviour(SteeringAgent agent)
        {
            SteeringDataCache.Reset();
            var mostPriminentHit = GetMostProminentObstacle(agent);

            if(!mostPriminentHit.HasValue) {
                return SteeringDataCache;
            }

            var weight = mostPriminentHit.Value.distance / LookAheadDistance;
            SteeringDataCache.MovementFromDirection(mostPriminentHit.Value.normal, 1f, weight * Priority);

            return SteeringDataCache;
        }


        public RaycastHit? GetMostProminentObstacle(SteeringAgent agent)
        {
            if(Physics.Raycast(agent.transform.position, agent.transform.forward, out var rayCastHit, LookAheadDistance)) {
                if (rayCastHit.collider.GetComponent<SteeringObstacle>() != null) {
                    return rayCastHit;
                }
            }

            return null;
        }
    }
}