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
        public List<SteeringBehaviourBase> SteeringBehaviours = new List<SteeringBehaviourBase>();
        public Transform Target;

        [HideInInspector]
        public SteeringData CurrentSteeringData;
        [HideInInspector]
        public SteeringData TargetSteeringData;
        [HideInInspector]
        public Vector3 CurrentMovementSpeed;
        [HideInInspector]
        public Vector3 CurrentOritation;

        [HideInInspector]
        public Vector3 StartPosition;

        [HideInInspector]
        public List<SteeringAgent> FriendlyAgents;

        public bool IsStandStill() => CurrentMovementSpeed.magnitude < (ClampMovement * ClampMovement);

        private void Start()
        {
            CurrentSteeringData = new SteeringData();
            TargetSteeringData = new SteeringData();

            FriendlyAgents = FindObjectsOfType<SteeringAgent>()
                .Where(a => a != this)
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
            UpdateMovement();
            UpdateOritation();
        }

        private void UpdateMovement()
        {
            var movementDirection = GetMovementDirection();

            if (movementDirection.sqrMagnitude < (ClampMovement * ClampMovement)) {
                movementDirection = Vector3.zero;
            }

            CurrentMovementSpeed = Vector3.Lerp(CurrentMovementSpeed, movementDirection * MovementSpeed, 0.1f);
            transform.position += (CurrentMovementSpeed * Time.deltaTime);
        }

        private void UpdateOritation()
        {
            var orientationDirection = GetOrientationDirection();
            CurrentOritation = Vector3.Slerp(CurrentOritation, orientationDirection, 0.1f);
            transform.rotation = Quaternion.LookRotation(CurrentOritation);
        }

        private Vector3 GetMovementDirection()
        {
            return CurrentSteeringData.MovementMax().CappedMovementDirection();
        }

        private Vector3 GetOrientationDirection()
        {
            return CurrentSteeringData.OrientationMax().CappedOrientationDirection();
        }

        private void UpdateTargetDirections()
        {
            TargetSteeringData.Reset();

            foreach (var behaviour in SteeringBehaviours) {
                var steeringData = behaviour.GetSteeringData();

                for (int i = 0; i < steeringData.Directions.Length; i++) {
                    TargetSteeringData.Directions[i].MovementWeight += steeringData.Directions[i].MovementWeight;
                    TargetSteeringData.Directions[i].OrientationWeight += steeringData.Directions[i].OrientationWeight;
                }
            }

            TargetSteeringData.BalanceMovement();
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

                if (direction.MovementWeight > 0) {
                    Debug.DrawLine(startPosition, startPosition + direction.Direction * direction.MovementWeight, Color.Lerp(Color.yellow, Color.green, direction.MovementWeight));
                } else {
                    Debug.DrawLine(startPosition, startPosition + direction.Direction * Mathf.Abs(direction.MovementWeight), Color.red);
                }
            }
        }
    }
}