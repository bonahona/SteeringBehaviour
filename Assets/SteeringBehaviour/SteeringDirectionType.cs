using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public enum SteeringDirectionType : byte
    {
        North = 0,
        NorthNorthEast = 1,
        NorthEast = 2,
        EastNorthEast = 3,
        East = 4,
        EastSouthEast = 5,
        SouthEast = 6,
        SouthSouthEast = 7,
        South = 8,
        SouthSouthWest = 9,
        SouthWest = 10,
        WestSouthWest = 11,
        West = 12,
        WestNorthWest = 13,
        NorthWest = 14,
        NorthNorthWest = 15
    }

    public static class SteeringUtils
    {
        public const int SteeringDirectionCount = 16;

        public static readonly Vector3[] SteeringDirection;

        static SteeringUtils()
        {
            SteeringDirection = new Vector3[] {
                Vector3.forward,
                (Vector3.forward + Vector3.forward + Vector3.right).normalized,
                (Vector3.forward + Vector3.right).normalized,
                (Vector3.forward + Vector3.right + Vector3.right).normalized,
                Vector3.right,
                (Vector3.back + Vector3.right + Vector3.right).normalized,
                (Vector3.back + Vector3.right).normalized,
                (Vector3.back + Vector3.back + Vector3.right).normalized,
                Vector3.back,
                (Vector3.back + Vector3.back + Vector3.left).normalized,
                (Vector3.back + Vector3.left).normalized,
                (Vector3.back + Vector3.left + Vector3.left).normalized,
                Vector3.left,
                (Vector3.forward + Vector3.left + Vector3.left).normalized,
                (Vector3.forward + Vector3.left).normalized,
                (Vector3.forward + Vector3.forward + Vector3.left).normalized
            };
        }
    }
}