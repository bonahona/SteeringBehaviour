using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(Rigidbody))]
    public class SteeringAgent : MonoBehaviour
    {
        public bool UseAgent = true;        // Reserved for players
        public float MovementSpeed = 2;
        public float Radius = 1f;

        public BehaviourContainer Behaviour;
        public SteeringAgent Target;

        public TeamType Team;

        [HideInInspector]
        public Rigidbody Rigidbody;
        [HideInInspector]
        public SteeringData CurrentSteeringData;
        [HideInInspector]
        public SteeringData TargetSteeringData;
        [HideInInspector]
        public Vector3 CurrentMovementSpeed;
        [HideInInspector]
        public Vector3 CurrentOrienttion;
        [HideInInspector]
        public NavMeshPath NavMeshPath;
        [HideInInspector]
        public int CurrentNavMeshPathIndex = 0;
        [HideInInspector]
        public float RepathTimer = 0f;

        [HideInInspector]
        public Vector3 StartPosition;

        [HideInInspector]
        public List<SteeringAgent> FriendlyAgents;

        public bool ActiveTarget() => Target?.isActiveAndEnabled ?? false;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            StartPosition = transform.position;
            CurrentSteeringData = new SteeringData();
            TargetSteeringData = new SteeringData();

            NavMeshPath = new NavMeshPath();

            FriendlyAgents = FindObjectsOfType<SteeringAgent>()
                .Where(a => a != this)
                .Where(a => a.Team == Team)
                .ToList();

            if(Behaviour == null) {
                return;
            }

            foreach(var behaviour in Behaviour.Behaviours) {
                if(behaviour == null) {
                    continue;
                }

                behaviour.StartBehaviour(this);
            }
        }

        private void FixedUpdate()
        {
            if(!UseAgent) {
                return;
            }

            UpdateSteeringBehaviour();
            MoveAgent();
        }

        private void UpdateSteeringBehaviour()
        {
            if((Behaviour?.Behaviours.Count ?? 0) == 0) {
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

            if (movementDirection.magnitude < (Behaviour?.ClampMovement ?? 0)) {
                movementDirection = Vector3.zero;
            }

            CurrentMovementSpeed = Vector3.Lerp(CurrentMovementSpeed, movementDirection * MovementSpeed, 0.1f);

            Rigidbody.MovePosition(transform.position + CurrentMovementSpeed * Time.fixedDeltaTime);
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

            foreach (var behaviour in Behaviour.Behaviours) {
                if (behaviour == null || !behaviour.Enabled) {
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