using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public class DirectionData
    {
        public Vector3 Direction;
        public float Weight;

        public Vector3 CappedDirection()
        {
            if(Weight < 1f) {
                return Direction * Weight;
            } else {
                return Direction;
            }
        }

        public override string ToString() => $"{Direction} - {Weight}";
    }

    public class SteeringData
    {
        public DirectionData[] Directions;

        public SteeringData()
        {
            Directions = new DirectionData[SteeringUtils.SteeringDirectionCount];
            for (int i = 0; i < SteeringUtils.SteeringDirectionCount; i++) { 
                Directions[i] = new DirectionData {  Direction = SteeringUtils.SteeringDirection[i], Weight = 0 };
            }
        }

        public void Reset()
        {
            foreach(var direction in Directions) {
                direction.Weight = 0;
            }
        }

        public DirectionData Max()
        {
            var resultIndex = 0;

            for(int i = 0; i < Directions.Length; i ++) {
                if(Directions[i].Weight > Directions[resultIndex].Weight) {
                    resultIndex = i;
                }
            }

            return Directions[resultIndex];
        }

        public void FromDirection(Vector3 direction, float backwardbackwardsFactor = 1f, float weight = 1f)
        {
            for (int i = 0; i < Directions.Length; i++) {
                var dot = Vector3.Dot(SteeringUtils.SteeringDirection[i], direction);
                if(dot < 0) {
                    dot *= backwardbackwardsFactor;
                }

                Directions[i].Weight = dot * weight;
            }
        }

        public void Apply(SteeringData other)
        {
            for (int i = 0; i < Directions.Length; i++) {
                Directions[i].Weight += other.Directions[i].Weight;
            }
        }
    }
}