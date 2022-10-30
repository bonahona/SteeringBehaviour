using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Fyrvall.SteeringBehaviour.Data
{
    public class NavMeshSteeringData
    {
        public bool HasPath = false;
        public int NextNavMeshPathIndex = 0;
        public float RepathTimer = 0f;

        public NavMeshPath NavMeshPath = new NavMeshPath();
        public List<float> SegmentLengthCache = new List<float>();

        public bool CanUsePath()
        {
            if(!HasPath) {
                return false;
            }

            if(NavMeshPath.status != NavMeshPathStatus.PathComplete) {
                return false;
            }

            if(NextNavMeshPathIndex >= NavMeshPath.corners.Length) {
                return false;
            }

            return true;
        }
        public void SetRepathTimer(float timer)
        {
            RepathTimer = timer;
        }

        public void Repath(Vector3 startPosition, Vector3 targetPosition)
        {
            if (!NavMesh.CalculatePath(startPosition, targetPosition, int.MaxValue, NavMeshPath)) {
                HasPath = false;
            }

            NextNavMeshPathIndex = 0;

            HasPath = true;
            SegmentLengthCache.Clear();

            for(int i = 0; i < NavMeshPath.corners.Length -1; i ++) {
                SegmentLengthCache.Add((NavMeshPath.corners[i] - NavMeshPath.corners[i +1]).magnitude);
            }
        }

        public float GetTotalPathDistanceLeft(SteeringAgent agent)
        {
            var result = 0f;

            if (!CanUsePath()) {
                return result;
            }

            for (int i = 0; i < NavMeshPath.corners.Length -1; i++) {
                if(i < agent.NavMeshSteeringData.NextNavMeshPathIndex) { 
                    if(i + 1 == agent.NavMeshSteeringData.NextNavMeshPathIndex) {
                        result += (agent.transform.position - NavMeshPath.corners[i +1]).magnitude;
                    }
                } else {
                    result += SegmentLengthCache[i];
                }
            }

            return result;
        }
    }
}