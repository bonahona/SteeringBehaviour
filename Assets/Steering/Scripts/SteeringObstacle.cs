using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(Collider))]
    public class SteeringObstacle : MonoBehaviour
    {
        private void Start()
        {
            AvoidObstacleBehaviour.RegisterObstacle(this);    
        }

        public SteeringData SteeringAway(SteeringAgent agent)
        {
            return new SteeringData();
        }
    }
}