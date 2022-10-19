using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class AvoidHazardSteeringBehaviour : MonoBehaviour, ISteeringBehaviour
    {
        public float Priority = 1f;

        private SteeringAgent SteeringAgent;
        private SteeringData Data = new SteeringData();

        private List<SteeringHazard> Hazards;
        private List<SteeringData> HazardSteeringData;

        private void Start()
        {
            SteeringAgent = GetComponent<SteeringAgent>();
            Hazards = GameObject.FindObjectsOfType<SteeringHazard>().ToList();
            HazardSteeringData = Hazards.Select(_ => new SteeringData()).ToList();
        }

        public void DebugDraw()
        {
            Debug.DrawLine(transform.position, transform.position + Data.Movement * Data.Weight, Color.blue);
        }

        public float GetPriority() => Priority;

        public SteeringData GetSteeringData()
        {
            return Data;
        }

        public void UpdateBehaviour()
        {
            Data.Weight = 0;
            Data.Movement = Vector3.zero;

            for(int i = 0; i < Hazards.Count; i ++) {
                Hazards[i].GetSteeringForAgent(SteeringAgent, HazardSteeringData[i]);

                Data.Weight += HazardSteeringData[i].Weight;
                Data.Movement += HazardSteeringData[i].Movement;
            }
        }
    }
}