using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public abstract class SteeringMovementBase : MonoBehaviour
    {
        public abstract void MoveAgent(Vector3 targetMovementDirection);
        public abstract void OrientAgent(Vector3 targetOrientationDirection);
    }
}