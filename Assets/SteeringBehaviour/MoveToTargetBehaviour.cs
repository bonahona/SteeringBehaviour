using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class MoveToTargetBehaviour : MonoBehaviour, ISteeringBehaviour
    {
        public Transform Target;
        public float DesiredDistance = 1f;
        public float Priority = 1f;

        private SteeringData Data = new SteeringData();

        public void UpdateBehaviour()
        {
            if (Target == null) {
                Data.Movement = Vector3.zero;
                Data.Orientation = Vector3.zero;
                Data.Weight = 0f;
            } else {
                var distance = (Target.position - transform.position);
                Data.Weight = Mathf.Clamp01(distance.sqrMagnitude - DesiredDistance);
                Data.Movement = distance.normalized;
                Data.Orientation = distance.normalized;
            }
        }

        public void DebugDraw()
        {
            Debug.DrawLine(transform.position, transform.position + Data.Movement * Data.Weight, Color.green);
        }

        public SteeringData GetSteeringData() => Data;
        public float GetPriority() => Priority;
    }
}