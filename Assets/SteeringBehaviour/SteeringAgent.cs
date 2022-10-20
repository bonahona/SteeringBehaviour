using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(Rigidbody))]
    public class SteeringAgent : MonoBehaviour
    {
        public bool UseAgent = true;        // Reserved for players
        public float MovementSpeed = 2;

        public List<ISteeringBehaviour> SteeringBehaviours = new List<ISteeringBehaviour>();

        [System.NonSerialized]
        public SteeringData CurrentSteeringData;
        [System.NonSerialized]
        public SteeringData TargetSteeringData;

        private Vector3 CurrentMovementSpeed;
        private Rigidbody Rigidbody;

        public bool IsStandStill() => CurrentMovementSpeed.magnitude < 0.001f;

        private void Start()
        {
            CurrentSteeringData = new SteeringData();
            TargetSteeringData = new SteeringData();

            Rigidbody = GetComponent<Rigidbody>();
            SteeringBehaviours = GetComponents<MonoBehaviour>()
                .Where(c => c is ISteeringBehaviour)
                .Cast<ISteeringBehaviour>()
                .ToList();
        }

        private void Update()
        {
            if(!UseAgent) {
                return;
            }

            UpdateSteeringBehaviour();
            MoveAgent();
        }

        private void UpdateSteeringBehaviour()
        {
            if(SteeringBehaviours.Count == 0) {
                return;
            }

            foreach(var behaviour in SteeringBehaviours) {
                behaviour.UpdateBehaviour();
            }

            UpdateTargetDirections();
            UpdateCurrentDirections();
        }

        private void MoveAgent()
        {
            var movementDirection = GetMovementDirection();

            CurrentMovementSpeed = Vector3.Lerp(CurrentMovementSpeed, movementDirection * MovementSpeed, 0.1f);
            Rigidbody.MovePosition(transform.position + CurrentMovementSpeed * Time.fixedDeltaTime);
        }

        private Vector3 GetMovementDirection()
        {
            return CurrentSteeringData.Max().CappedDirection();
        }

        private void UpdateTargetDirections()
        {
            TargetSteeringData.Reset();

            foreach (var behaviour in SteeringBehaviours) {
                var steeringData = behaviour.GetSteeringData();

                for (int i = 0; i < steeringData.Directions.Length; i++) {
                    TargetSteeringData.Directions[i].Weight += steeringData.Directions[i].Weight;
                }
            }
        }

        private void UpdateCurrentDirections()
        {
            for (int i = 0; i < CurrentSteeringData.Directions.Length; i++) {
                CurrentSteeringData.Directions[i] = TargetSteeringData.Directions[i];
            }
        }
    }
}