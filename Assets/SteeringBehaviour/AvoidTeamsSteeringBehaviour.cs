using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class AvoidTeamsSteeringBehaviour : MonoBehaviour, ISteeringBehaviour
    {
        public float DesiredDistance = 1f;

        [Range(0f, 5f)]
        public float Priority = 1f;

        [HideInInspector]
        public SteeringData SteeringData = new SteeringData();
        [HideInInspector]
        public SteeringData SteeringDataCache = new SteeringData();
        [HideInInspector]
        public SteeringAgent SteeringAgent;
        [HideInInspector]
        public List<SteeringAgent> FriendlyAgents;

        private void Start()
        {
            SteeringAgent = GetComponent<SteeringAgent>();
            FriendlyAgents = GameObject.FindObjectsOfType<SteeringAgent>()
                .Where(a => a != SteeringAgent)
                .ToList();
        }


        public SteeringData GetSteeringData() => SteeringData;

        public void UpdateBehaviour()
        {
            SteeringData.Reset();

            foreach (var agent in FriendlyAgents) {
                var delta = agent.transform.position - transform.position;

                if (delta.magnitude > DesiredDistance) {
                    continue;
                }

                var weight = 1f - (delta.magnitude / DesiredDistance);
                SteeringDataCache.MovementFromDirection(-delta.normalized, 1f, weight * Priority);
                SteeringData.Apply(SteeringDataCache);
            }
        }

        public int Index() => 1;
    }
}