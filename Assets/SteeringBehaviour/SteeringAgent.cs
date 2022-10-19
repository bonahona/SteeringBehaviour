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

        private Vector3 CurrentMovementSpeed;
        private Rigidbody Rigidbody;

        public bool IsStandStill() => CurrentMovementSpeed.magnitude < 0.001f;

        private void Start()
        {
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
                behaviour.DebugDraw();
            }

            var movementDirection = GetMovementDirection();
            CurrentMovementSpeed = Vector3.Lerp(CurrentMovementSpeed, movementDirection * MovementSpeed, 0.1f);
            Rigidbody.MovePosition(transform.position + CurrentMovementSpeed * Time.fixedDeltaTime);
        }

        private Vector3 GetMovementDirection()
        {
            var result = Vector3.zero;

            foreach(var behaviour in SteeringBehaviours) {
                var steeringData = behaviour.GetSteeringData();
                result += steeringData.Movement * steeringData.Weight * behaviour.GetPriority();
            }
            result.y = 0;
            return result.normalized;
        }
    }
}