using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class CircleTargetSteeringBehaviour : MonoBehaviour, ISteeringBehaviour
    {
        public static readonly Quaternion[] Directions = new Quaternion[]{
            Quaternion.Euler(0, -90, 0),
            Quaternion.Euler(0, 90, 0)
        };

        public enum DirectionType : byte
        {
            Left = 0,
            Right = 1
        }

        public Transform Target;
        public float DesiredDistance = 1f;

        [Range(0f, 5f)]
        public float Priority = 1f;

        [HideInInspector]
        public SteeringData SteeringData = new SteeringData();
        [HideInInspector]
        public SteeringData SteeringDataCache = new SteeringData();
        [HideInInspector]
        public DirectionType CurrentDirection;

        private void Start()
        {
            CurrentDirection = (DirectionType)Random.Range(0, 2);
        }

        public void UpdateBehaviour()
        {
            SteeringData.Reset();
            if (Target == null) {
                return;
            }

            var delta = (Target.position - transform.position);
            var distance = delta.magnitude;
            if (distance > DesiredDistance) {
                return;
            }

            SteeringData.MovementFromDirection(Directions[(byte)CurrentDirection] * delta.normalized, 0f, Priority);
        }

        public SteeringData GetSteeringData() => SteeringData;
    }
}