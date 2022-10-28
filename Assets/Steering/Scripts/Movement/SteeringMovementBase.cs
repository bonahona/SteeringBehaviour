using UnityEngine;

namespace Fyrvall.SteeringBehaviour.Movement
{
    public abstract class SteeringMovementBase : MonoBehaviour
    {
        public void UpdateAgent(SteeringAgent steeringAgent, float deltaTime)
        {
            steeringAgent.UpdateAgent(deltaTime);
        }

        public abstract void MoveAgent(Vector3 targetMovementDirection);
        public abstract void OrientAgent(Vector3 targetOrientationDirection);
    }
}