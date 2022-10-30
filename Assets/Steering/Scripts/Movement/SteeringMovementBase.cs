using UnityEngine;

namespace Fyrvall.SteeringBehaviour.Movement
{
    public abstract class SteeringMovementBase : MonoBehaviour
    {
        public float MovementSpeed = 10;
        public float Acceleration = 50;
        public float RotationSpeed = 360;

        protected SteeringAgent Agent;
        protected Vector3 CurrentMovementSpeed;
        protected Vector3 CurrentOriention;

        public void UpdateAgent(SteeringAgent steeringAgent, float deltaTime)
        {
            steeringAgent.UpdateAgent(deltaTime);
        }

        public abstract void MoveAgent(Vector3 targetMovementDirection);
        public abstract void OrientAgent(Vector3 targetOrientationDirection);
    }
}