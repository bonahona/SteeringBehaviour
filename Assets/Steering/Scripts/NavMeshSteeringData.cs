using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Fyrvall.SteeringBehaviour
{
    public class NavMeshSteeringData
    {
        public bool HasPath = false;
        public int CurrentNavMeshPathIndex = 0;
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

            if(CurrentNavMeshPathIndex >= NavMeshPath.corners.Length) {
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

            CurrentNavMeshPathIndex = 0;

            HasPath = true;
            SegmentLengthCache.Clear();

            for(int i = 0; i < NavMeshPath.corners.Length -1; i ++) {
                SegmentLengthCache.Add((NavMeshPath.corners[i] - NavMeshPath.corners[i +1]).magnitude);
            }
        }

        public float GetTotalPathDistance(SteeringAgent agent)
        {
            var result = 0f;

            if (!CanUsePath()) {
                return result;
            }

            for (int i = 0; i < NavMeshPath.corners.Length -1; i++) {
                if (CurrentNavMeshPathIndex == i) {
                    result += (agent.transform.position - NavMeshPath.corners[i]).magnitude;
                } else {
                    result += SegmentLengthCache[i];
                }
            }

            return result;
        }
    }
}