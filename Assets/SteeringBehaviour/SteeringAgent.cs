using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(Rigidbody))]
    public class SteeringAgent : MonoBehaviour
    {
        public float MovementSpeed = 2;

        public List<ISteeringBehaviour> SteeringBehaviours = new List<ISteeringBehaviour>();

        private SteeringData SteeringData;
        private Vector3 CurrentMovementSpeed;
        private Rigidbody Rigidbody;

        public bool IsStandStill() => CurrentMovementSpeed.magnitude < 0.001f;

        private void Start()
        {
            SteeringData = new SteeringData();
            Rigidbody = GetComponent<Rigidbody>();
            SteeringBehaviours = GetComponents<MonoBehaviour>()
                .Where(c => c is ISteeringBehaviour)
                .Cast<ISteeringBehaviour>()
                .ToList();
        }

        private void Update()
        {
            UpdateSteeringBehaviour();
        }

        private void UpdateSteeringBehaviour()
        {
            if(SteeringBehaviours.Count == 0) {
                return;
            }

            foreach(var behaviour in SteeringBehaviours) {
                behaviour.UpdateBehaviour();
            }

            var movementDirection = GetMovementDirection();
            CurrentMovementSpeed = Vector3.Lerp(CurrentMovementSpeed, movementDirection * MovementSpeed, 0.1f);
            Rigidbody.MovePosition(transform.position + CurrentMovementSpeed * Time.fixedDeltaTime);
        }

        private Vector3 GetMovementDirection()
        {
            SteeringData.Reset();

            foreach(var behaviour in SteeringBehaviours) {
                var steeringData = behaviour.GetSteeringData();
                
                for(int i = 0; i < steeringData.Directions.Length; i ++) {
                    SteeringData.Directions[i] += steeringData.Directions[i];
                }
            }

            for (int i = 0; i < SteeringData.Directions.Length; i++) {
                Debug.DrawLine(transform.position, transform.position + SteeringData.Directions[i], Color.green);
            }



            return SteeringData.Max();
        }
    }
}