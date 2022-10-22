using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public enum SteeringDirectionType : byte
    {
        None = 0,
        North = 1,
        NorthNorthEast = 2,
        NorthEast = 3,
        EastNorthEast = 4,
        East = 5,
        EastSouthEast = 6,
        SouthEast = 7,
        SouthSouthEast = 8,
        South = 9,
        SouthSouthWest = 10,
        SouthWest = 11,
        WestSouthWest = 12,
        West = 13,
        WestNorthWest = 14,
        NorthWest = 15,
        NorthNorthWest = 16
    }

    public static class SteeringUtils
    {
        public const int SteeringDirectionCount = 17;

        public static readonly Vector3[] SteeringDirection;
        public static int[] OppositeIndex;

        static SteeringUtils()
        {
            SteeringDirection = new Vector3[] {
                Vector3.zero,
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

            OppositeIndex = new int[] {
                0,
                9,
                10,
                11,
                12,
                13,
                14,
                15,
                16,
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8
            };
        }
    }
}