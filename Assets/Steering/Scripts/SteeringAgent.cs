using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Fyrvall.SteeringBehaviour
{
    public class SteeringAgent : MonoBehaviour
    {
        public bool UseAgent = true;        // Reserved for players
        public float MovementSpeed = 2;
        public float Radius = 1f;

        [Range(0f, 5f)]
        public float ClampMovement = 0.1f;
        public List<SteeringBehaviourBase> SteeringBehaviours = new List<SteeringBehaviourBase>();
        public SteeringAgent Target;

        public TeamType Team;

        [HideInInspector]
        public SteeringData CurrentSteeringData;
        [HideInInspector]
        public SteeringData TargetSteeringData;
        [HideInInspector]
        public Vector3 CurrentMovementSpeed;
        [HideInInspector]
        public Vector3 CurrentOrienttion;

        [HideInInspector]
        public Vector3 StartPosition;

        [HideInInspector]
        public List<SteeringAgent> FriendlyAgents;

        public bool IsStandStill() => CurrentMovementSpeed.magnitude < (ClampMovement * ClampMovement);
        public bool ActiveTarget() => Target?.isActiveAndEnabled ?? false;

        private void Start()
        {
            StartPosition = transform.position;
            CurrentSteeringData = new SteeringData();
            TargetSteeringData = new SteeringData();

            FriendlyAgents = FindObjectsOfType<SteeringAgent>()
                .Where(a => a != this)
                .Where(a => a.Team == Team)
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
            CurrentOrienttion = Vector3.Slerp(CurrentOrienttion, orientationDirection, 0.1f);
            if (CurrentOrienttion.sqrMagnitude > 0.1f) {
                transform.rotation = Quaternion.LookRotation(CurrentOrienttion);
            }
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
                if (behaviour == null) {
                    continue;
                }

                var steeringData = behaviour.UpdateBehaviour(this);

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