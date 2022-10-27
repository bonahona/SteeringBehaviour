using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        public SteeringData CurrentSteeringData;
        [HideInInspector]
        public SteeringData TargetSteeringData;
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

        private SteeringMovementBase Movement;

        public bool ActiveTarget() => Target?.isActiveAndEnabled ?? false;

        private void Start()
        {
            Movement = GetComponent<SteeringMovementBase>();

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
        }

        private void FixedUpdate()
        {
            if (UseAgent) {
                UpdateSteeringBehaviour();
                UpdateAgentValues();
            }

            Movement.MoveAgent(TargetMovementSpeed);
            Movement.OrientAgent(TargetOrientation);
        }

        private void UpdateSteeringBehaviour()
        {
            UpdateTargetDirections();
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