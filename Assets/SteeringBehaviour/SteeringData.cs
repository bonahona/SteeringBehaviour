using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public class SteeringData
    {
        public Vector3[] Directions = new Vector3[SteeringUtils.SteeringDirectionCount];

        public void Reset()
        {
            for(int i = 0; i < Directions.Length; i ++) {
                Directions[i] = Vector3.zero;
            }
        }

        public Vector3 Max()
        {
            var result = Vector3.zero;

            for(int i = 0; i < Directions.Length; i ++) {
                if(Directions[i].sqrMagnitude > result.sqrMagnitude) {
                    result = Directions[i];
                }
            }

            return result;
        }

        public void FromDirection(Vector3 direction, float weight = 1f)
        {
            for (int i = 0; i < Directions.Length; i++) {
                var dot = Vector3.Dot(SteeringUtils.SteeringDirection[i], direction);
                Directions[i] = Mathf.Clamp01(dot) * SteeringUtils.SteeringDirection[i] * weight;
            }
        }

        public void Apply(SteeringData other)
        {
            for (int i = 0; i < Directions.Length; i++) {
                Directions[i] += other.Directions[i];
            }
        }

        public void Clamp01()
        {
            for (int i = 0; i < Directions.Length; i++) {
                if (Directions[i].sqrMagnitude > 1) {
                    Directions[i].Normalize();
                }
            }
        }
    }
}