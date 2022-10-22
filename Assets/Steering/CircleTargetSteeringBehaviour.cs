using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "CircleTargetBehaviour", menuName = "Steering/Circle Target")]
    public class CircleTargetSteeringBehaviour: SteeringBehaviourBase
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

        public float DesiredDistance = 1f;

        [Range(0f, 5f)]
        public float Priority = 1f;

        public override void UpdateBehaviour(SteeringAgent agent, SteeringData steeringData)
        {
            steeringData.Reset();
            //if (Target == null) {
            //    return;
            //}

            //var delta = (Target.position - transform.position);
            //var distance = delta.magnitude;
            //if (distance > DesiredDistance) {
            //    return;
            //}

            //SteeringData.MovementFromDirection(Directions[(byte)CurrentDirection] * delta.normalized, 0f, Priority);
        }
    }
}