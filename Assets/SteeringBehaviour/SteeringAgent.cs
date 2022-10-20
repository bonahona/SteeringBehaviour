using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Fyrvall.SteeringBehaviour
{
    public class SteeringAgent : MonoBehaviour
    {
        public bool UseAgent = true;        // Reserved for players
        public float MovementSpeed = 2;

        [Range(0f, 5f)]
        public float ClampMovement = 0.1f;

        public List<ISteeringBehaviour> SteeringBehaviours = new List<ISteeringBehaviour>();

        [HideInInspector]
        public SteeringData CurrentSteeringData;
        [HideInInspector]
        public SteeringData TargetSteeringData;
        [HideInInspector]
        public Vector3 CurrentMovementSpeed;

        public bool IsStandStill() => CurrentMovementSpeed.magnitude < 0.001f;

        private void Start()
        {
            CurrentSteeringData = new SteeringData();
            TargetSteeringData = new SteeringData();

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

            if(movementDirection.sqrMagnitude < (ClampMovement * ClampMovement)) {
                movementDirection = Vector3.zero;
            }

            CurrentMovementSpeed = Vector3.Lerp(CurrentMovementSpeed, movementDirection * MovementSpeed, 0.1f);
            transform.Translate(CurrentMovementSpeed * Time.deltaTime);
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

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) {
                return;
            }

            foreach (var direction in CurrentSteeringData.Directions) {
                var startPosition = transform.position + direction.Direction * 0.5f;

                if (direction.Weight > 0) {
                    Debug.DrawLine(startPosition, startPosition + direction.Direction * direction.Weight, Color.Lerp(Color.yellow, Color.green, direction.Weight));
                } else {
                    Debug.DrawLine(startPosition, startPosition + direction.Direction * Mathf.Abs(direction.Weight), Color.red);
                }
            }
        }
    }
}