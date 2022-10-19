using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class AvoidFriendliesSteeringBehaviour : MonoBehaviour, ISteeringBehaviour
    {
        public float DesiredDistance = 2f;
        public bool PrioritizeStanding = true;
        public float Priority = 1f;

        private List<SteeringAgent> Friendlies = new List<SteeringAgent>();

        private SteeringAgent SteeringAgent;
        private SteeringData Data = new SteeringData();
        private List<SteeringData> FriendlySteering = new List<SteeringData>();

        void Start()
        {
            SteeringAgent = GetComponent<SteeringAgent>();
            Friendlies = GameObject.FindObjectsOfType<SteeringAgent>().ToList();
            foreach(var friendly in Friendlies) {
                FriendlySteering.Add(new SteeringData());
            }
        }

        public void DebugDraw()
        {
            foreach(var friendlySteering in FriendlySteering) {
                if(friendlySteering.Weight == 0) {
                    continue;
                }

                Debug.DrawLine(transform.position, transform.position + friendlySteering.Movement * friendlySteering.Weight, Color.yellow);
            }
        }

        public SteeringData GetSteeringData () => Data;
        public float GetPriority() => Priority;

        public void UpdateBehaviour()
        {
            Data.Movement = Vector3.zero;

            if(SteeringAgent.IsStandStill()) {
                return;
            }

            for(int i = 0; i < Friendlies.Count; i ++) {
                var friendly = Friendlies[i];
                var friendlySteering = FriendlySteering[i];

                if(friendly == this) {
                    continue;
                }

                var deltaDistance = friendly.transform.position - transform.position;
                var distance = deltaDistance.magnitude;

                if(distance >= DesiredDistance) {
                    friendlySteering.Weight = 0;
                    friendlySteering.Movement = Vector3.zero;
                    continue;
                }

                friendlySteering.Weight = Mathf.Clamp01(1f - (distance / DesiredDistance));
                friendlySteering.Movement = -deltaDistance.normalized * friendlySteering.Weight;
            }

            Data.Weight = FriendlySteering.Sum(s => s.Weight);
            foreach (var friendlySteering in FriendlySteering) {
                Data.Movement += friendlySteering.Movement * friendlySteering.Weight;
            }

            Data.Movement.Normalize();
        }
    }
}