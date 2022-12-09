using UnityEngine;
using UnityEngine.AI;

namespace Fyrvall.SteeringBehaviour.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshAgentMovement : SteeringMovementBase
    {
        private NavMeshAgent NavMeshAgent;

        private void Start()
        {
            CurrentOriention = transform.forward;
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Agent = GetComponent<SteeringAgent>();
            if(Agent == null) {
                Debug.LogError($"{name} has a {nameof(SteeringBehaviourBase)} component but missing a {nameof(SteeringAgent)}");
            }
        }

        private void Update()
        {
            Agent.UpdateAgent(Time.deltaTime);
        }

        public override void MoveAgent(Vector3 targetMovementDirection)
        {
            CurrentMovementSpeed = Vector3.MoveTowards(CurrentMovementSpeed, targetMovementDirection * MovementSpeed, Acceleration * Time.deltaTime);
            NavMeshAgent.Move(CurrentMovementSpeed * Time.deltaTime);
        }

        public override void OrientAgent(Vector3 targetOrientationDirection)
        {
            CurrentOriention = Vector3.RotateTowards(CurrentOriention, targetOrientationDirection, RotationSpeed * Time.fixedDeltaTime, 0f);
            if (CurrentOriention.sqrMagnitude > 0.1f) {
                transform.rotation = Quaternion.LookRotation(CurrentOriention);
            }
        }
    }
}