using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class MoveToTargetBehaviour : MonoBehaviour, ISteeringBehaviour
    {
        public Transform Target;
        public float DesiredDistance = 1f;

        private SteeringData SteeringData = new SteeringData();

        public void UpdateBehaviour()
        {
            SteeringData.Reset();
            if (Target != null) {
                var delta = (Target.position - transform.position);
                
                if(delta.magnitude < DesiredDistance) {
                    return;
                }

                var direction = delta.normalized;
                SteeringData.FromDirection(direction);
            }
        }

        public SteeringData GetSteeringData() => SteeringData;
    }
}