using Fyrvall.SteeringBehaviour.Data;
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

        public DirectionType Direction = DirectionType.Left;

        public override SteeringData UpdateBehaviour(SteeringAgent agent)
        {
            SteeringDataCache.Reset();
            if (!agent.ActiveTarget()) {
                return SteeringDataCache;
            }

            var delta = (agent.Target.transform.position - agent.transform.position);
            var distance = delta.magnitude;
            if (distance > DesiredDistance) {
                return SteeringDataCache;
            }

            SteeringDataCache.MovementFromDirection(Directions[(byte)Direction] * delta.normalized, 0, Priority);
            SteeringDataCache.OrientationFromDirection(Directions[(byte)Direction] * delta.normalized, 0, Priority);
            return SteeringDataCache;
        }
    }
}