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
        public SteeringAgent Target;
        public TeamType Team;

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
        private SteeringFrameData SteeringFrameData;

        private SteeringMovementBase Movement;

        public bool ActiveTarget() => Target?.isActiveAndEnabled ?? false;

        private void Start()
        {
            Movement = GetComponent<SteeringMovementBase>();

            StartPosition = transform.position;
            CurrentSteeringData = new SteeringData();
            TargetSteeringData = new SteeringData();
            SteeringFrameData = new SteeringFrameData(Behaviour);

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
            if (!UseAgent) {
                return;
            }

            UpdateMovement();
            UpdateOritation();
        }

        private void UpdateMovement()
        {
            TargetMovementSpeed = GetMovementDirection();

            if (TargetMovementSpeed.magnitude < (Behaviour?.ClampMovement ?? 0)) {
                TargetMovementSpeed = Vector3.zero;
            }
        }

        private void UpdateOritation()
        {
            TargetOrientation = GetOrientationDirection();          
        }

        private Vector3 GetMovementDirection()
        {
            return CurrentSteeringData.MovementMax().CappedMovementDirection();
        }

        private Vector3 GetOrientationDirection()
        {
            return CurrentSteeringData.OrientationMax().CappedOrientationDirection();
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

                SteeringFrameData.RegisterSteeringData(behaviour, steeringData);
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

        public void CopyFrameData(SteeringFrameData other)
        {
            SteeringFrameData.Copy(other);
        }
    }
}