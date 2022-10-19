using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class AvoidTargetSteeringBehaviour : MonoBehaviour, ISteeringBehaviour
    {
        public Transform Target;
        public float AvoidDistance = 1f;
        public float Priority = 1f;

        private SteeringData Data = new SteeringData();

        public void UpdateBehaviour()
        {
            Data.Movement = Vector3.zero;
            Data.Orientation = Vector3.zero;
            Data.Weight = 0f;

            if (Target == null) {
                return;
            } else {
                var deltaDistance = (Target.position - transform.position);
                var distance = deltaDistance.magnitude;
                if(distance > AvoidDistance) {
                    return;
                }

                Data.Weight = AvoidDistance - distance;
                Data.Movement = -deltaDistance.normalized;
                Data.Orientation = deltaDistance.normalized;
            }
        }

        public void DebugDraw()
        {
            Debug.DrawLine(transform.position, transform.position + Data.Movement * Data.Weight, Color.red);
        }

        public SteeringData GetSteeringData() => Data;
        public float GetPriority() => Priority;
    }
}