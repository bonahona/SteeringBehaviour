using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [System.Serializable]
    public class DirectionData
    {
        public Vector3 Direction;
        public float MovementWeight;
        public float OrientationWeight;

        public float MovementWeightCache;

        public Vector3 CappedMovementDirection()
        {
            if(MovementWeight < 1f) {
                return Direction * MovementWeight;
            } else {
                return Direction;
            }
        }

        public Vector3 CappedOrientationDirection()
        {
            if (OrientationWeight < 1f) {
                return Direction * OrientationWeight;
            } else {
                return Direction;
            }
        }

        public override string ToString() => $"{Direction} - M:{MovementWeight} - O:{OrientationWeight}";
    }

    [System.Serializable]
    public class SteeringData
    {
        public DirectionData[] Directions;

        public SteeringData()
        {
            Directions = new DirectionData[SteeringUtils.SteeringDirectionCount];
            for (int i = 0; i < SteeringUtils.SteeringDirectionCount; i++) { 
                Directions[i] = new DirectionData {  Direction = SteeringUtils.SteeringDirection[i], MovementWeight = 0, OrientationWeight = 0 };
            }
        }

        public void Reset()
        {
            foreach(var direction in Directions) {
                direction.MovementWeight = 0;
                direction.OrientationWeight = 0;
                direction.MovementWeightCache = 0;
            }
        }

        public DirectionData MovementMax()
        {
            var resultIndex = 0;

            for(int i = 0; i < Directions.Length; i ++) {
                if(Directions[i].MovementWeight > Directions[resultIndex].MovementWeight) {
                    resultIndex = i;
                }
            }

            return Directions[resultIndex];
        }

        public DirectionData OrientationMax()
        {
            var resultIndex = 0;

            for (int i = 0; i < Directions.Length; i++) {
                if (Directions[i].OrientationWeight > Directions[resultIndex].OrientationWeight) {
                    resultIndex = i;
                }
            }

            return Directions[resultIndex];
        }

        public void MovementFromDirection(Vector3 fromDirection, float backwardbackwardsFactor = 1f, float weight = 1f)
        {
            foreach(var direction in Directions) {
                var dot = Vector3.Dot(direction.Direction, fromDirection);
                if (dot < 0) {
                    dot *= backwardbackwardsFactor;
                }

                direction.MovementWeight = dot * weight;
            }
        }

        public void OrientationFromDirection(Vector3 fromDirection, float backwardbackwardsFactor = 1f, float weight = 1f)
        {
            foreach (var direction in Directions) {
                var dot = Vector3.Dot(direction.Direction, fromDirection);
                if (dot < 0) {
                    dot *= backwardbackwardsFactor;
                }

                direction.OrientationWeight = dot * weight;
            }
        }

        public void CopyMovementToOrientation() {
            foreach(var direction in Directions) {
                direction.OrientationWeight = direction.MovementWeight;
            }
        }

        public void Apply(SteeringData other)
        {
            for (int i = 0; i < Directions.Length; i++) {
                Directions[i].MovementWeight += other.Directions[i].MovementWeight;
            }
        }

        public void BalanceMovement()
        {
            for(int i = 0; i < Directions.Length; i ++) {
                Directions[i].MovementWeightCache = Mathf.Max(Directions[i].MovementWeight - Directions[SteeringUtils.OppositeIndex[i]].MovementWeight, 0);
            }

            for (int i = 0; i < Directions.Length; i++) {
                Directions[i].MovementWeight = Directions[i].MovementWeightCache;
            }
        }
    }
}