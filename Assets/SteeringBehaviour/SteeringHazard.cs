using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public class SteeringHazard : MonoBehaviour
    {
        public float Range = 1f;
        public float AvoidanceValue = 1f;

        public void GetSteeringForAgent(SteeringAgent agent, SteeringData data)
        {
            var delta = agent.transform.position - transform.position;

            if(delta.magnitude > Range) {
                data.Weight = 0;
                data.Movement = Vector3.zero;
                return;
            }

            data.Weight = AvoidanceValue;
            data.Movement = delta.normalized;
        }
    }
}