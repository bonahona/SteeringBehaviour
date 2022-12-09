using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Fyrvall.SteeringBehaviour.Movement;
using Fyrvall.SteeringBehaviour.Data;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringMovementBase))]
    public class SteeringAgent : MonoBehaviour
    {
        public bool UseAgent = true;        // Reserved for players

        public BehaviourContainer Behaviour;
        public TargetProvider TargetProvider;

        public SteeringAgent Target;
        public TeamType Team;

        [Header("Debug")]
        public bool DebugDraw = false;

        [HideInInspector]
        public Vector3 TargetMovementSpeed;
        [HideInInspector]
        public Vector3 TargetOrientation;
        [HideInInspector]
        public NavMeshSteeringData NavMeshSteeringData;
        [HideInInspector]
        public Vector3 StartPosition;
        [HideInInspector]
        public List<SteeringAgent> FriendlyAgents;

        private SteeringData CurrentSteeringData;
        private SteeringData TargetSteeringData;

        private SteeringMovementBase Movement;

        public bool ActiveTarget() => Target?.isActiveAndEnabled ?? false;

        private void Start()
        {
            Movement = GetComponent<SteeringMovementBase>();

            TargetOrientation = transform.forward;

            StartPosition = transform.position;
            CurrentSteeringData = new SteeringData();
            TargetSteeringData = new SteeringData();

            NavMeshSteeringData = new NavMeshSteeringData();

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

            Target = GetTarget();
        }

        private SteeringAgent GetTarget()
        {
            return TargetProvider?.GetTarget(this);
        }

        public void UpdateAgent(float deltaTime)
        {
            if (UseAgent) {
                UpdateSteeringBehaviour(deltaTime);
                UpdateAgentValues();
            }

            Movement.MoveAgent(TargetMovementSpeed);
            Movement.OrientAgent(TargetOrientation);
        }

        private void UpdateSteeringBehaviour(float deltaTime)
        {
            UpdateTargetDirections(deltaTime);
            UpdateCurrentDirections();
        }

        private void UpdateAgentValues()
        {
            if (!UseAgent || Behaviour == null) {
                return;
            }

            TargetMovementSpeed = GetMovementDirection();

            if (TargetMovementSpeed.magnitude < (Behaviour.ClampMovement)) {
                TargetMovementSpeed = Vector3.zero;
            }

            TargetOrientation = GetOrientationDirection();
        }

        private Vector3 GetMovementDirection()
        {
            return CurrentSteeringData.MovementMax().CappedMovementDirection();
        }

        private Vector3 GetOrientationDirection()
        {
            var result = CurrentSteeringData.OrientationMax();

            if (result.OrientationWeight < Behaviour.ClampMovement) {
                return TargetOrientation;
            } else {
                return result.Direction;
            }
        }

        private void UpdateTargetDirections(float deltaTime)
        {
            TargetSteeringData.Reset();

            foreach (var behaviour in Behaviour.Behaviours) {
                if (behaviour == null || !behaviour.Enabled) {
                    continue;
                }

                var steeringData = behaviour.UpdateBehaviour(this, deltaTime);

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

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) {
                return;
            }

            if(Behaviour == null) {
                return;
            }

            foreach (var behaviour in Behaviour.Behaviours) {
                if (behaviour == null || !behaviour.Enabled) {
                    continue;
                }

                behaviour.DebugDraw(this);
            }
        }
    }
}