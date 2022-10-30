using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public static class SteeringUtils
    {
        public const int SteeringDirectionCount = 16;
        public const int DefaultSteeringIndex = SteeringDirectionCount - 1;

        public static readonly Vector3[] SteeringDirection;
        public static int[] OppositeIndex;

        static SteeringUtils()
        {
            var directionCount = SteeringDirectionCount;
            if (directionCount % 2 == 1) {
                Debug.LogWarning($"Direction count was uneven value {directionCount}, bumping to {directionCount + 1}");
                directionCount += 1;
            }

            SteeringDirection = new Vector3[directionCount + 1];     // Appends 1 for 0th index Vector.zero
            OppositeIndex = new int[directionCount + 1];

            SteeringDirection[directionCount] = Vector3.zero;
            OppositeIndex[directionCount] = directionCount;

            var stepRotation = 360f / directionCount;
            var halfCount = Mathf.CeilToInt(directionCount / 2);
            for (int i = 0; i < directionCount; i++) {
                SteeringDirection[i] = Quaternion.Euler(0, stepRotation * i, 0) * Vector3.forward;
                OppositeIndex[i] = Mathf.CeilToInt(Mathf.Repeat(i + halfCount, directionCount));
            }
        }
    }
}