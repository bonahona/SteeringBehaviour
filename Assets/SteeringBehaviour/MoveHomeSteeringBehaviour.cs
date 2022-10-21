using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class MoveHomeSteeringBehaviour : MonoBehaviour, ISteeringBehaviour
    {
        [Range(0f, 5f)]
        public float Priority = 1f;

        [HideInInspector]
        public SteeringData SteeringData = new SteeringData();
        [HideInInspector]
        public SteeringAgent SteeringAgent;
        [HideInInspector]
        public Vector3 StartPosition;

        private void Start()
        {
            SteeringAgent = GetComponent<SteeringAgent>();
            StartPosition = SteeringAgent.transform.position;
        }


        public SteeringData GetSteeringData() => SteeringData;

        public void UpdateBehaviour()
        {
            SteeringData.Reset();

            var delta = (StartPosition - transform.position);
            SteeringData.MovementFromDirection(delta.normalized * Priority, 1f);
        }
    }
}