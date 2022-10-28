using UnityEngine;
using UnityEngine.AI;

namespace Fyrvall.SteeringBehaviour.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshAgentMovement : SteeringMovementBase
    {
        public float MovementSpeed = 2;


        private NavMeshAgent NavMeshAgent;
        private SteeringAgent Agent;

        private Vector3 CurrentMovementSpeed;
        private Vector3 CurrentOrienttion;

        private void Start()
        {
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
            CurrentMovementSpeed = Vector3.Lerp(CurrentMovementSpeed, targetMovementDirection * MovementSpeed, 0.1f);
            NavMeshAgent.Move(CurrentMovementSpeed * Time.fixedDeltaTime);
        }

        public override void OrientAgent(Vector3 targetOrientationDirection)
        {
            CurrentOrienttion = Vector3.Slerp(CurrentOrienttion, targetOrientationDirection, 0.1f);
            if (CurrentOrienttion.sqrMagnitude > 0.1f) {
                transform.rotation = Quaternion.LookRotation(CurrentOrienttion);
            }
        }
    }
}