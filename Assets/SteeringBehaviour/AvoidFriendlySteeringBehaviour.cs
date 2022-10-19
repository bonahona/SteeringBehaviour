using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class AvoidFriendlySteeringBehaviour : MonoBehaviour, ISteeringBehaviour
    {
        public float DesiredDistance = 1f;

        private SteeringData SteeringData = new SteeringData();
        private SteeringData SteeringDataCache = new SteeringData();
        private SteeringAgent SteeringAgent;
        private List<SteeringAgent> FriendlyAgents;

        private void Start()
        {
            SteeringAgent = GetComponent<SteeringAgent>();
            FriendlyAgents = GameObject.FindObjectsOfType<SteeringAgent>().Where(a => a != SteeringAgent).ToList();
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

                SteeringDataCache.FromDirection(-delta.normalized, 1f - (delta.magnitude / DesiredDistance));
                SteeringData.Apply(SteeringDataCache);
            }
        }
    }
}