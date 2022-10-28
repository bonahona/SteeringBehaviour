using UnityEngine;

namespace Fyrvall.SteeringBehaviour.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMovement : SteeringMovementBase
    {
        public float MovementSpeed = 2;

        private Rigidbody Rigidbody;
        private SteeringAgent Agent;
        private Vector3 CurrentMovementSpeed;
        private Vector3 CurrentOrienttion;

        void Start()
        {
            Agent = GetComponent<SteeringAgent>();
            if (Agent == null) {
                Debug.LogError($"{name} has a {nameof(SteeringBehaviourBase)} component but missing a {nameof(SteeringAgent)}");
            }

            Rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Agent.UpdateAgent(Time.fixedDeltaTime);
        }

        public override void MoveAgent(Vector3 targetMovementDirection)
        {
            CurrentMovementSpeed = Vector3.Lerp(CurrentMovementSpeed, targetMovementDirection * MovementSpeed, 0.1f);
            Rigidbody.MovePosition(transform.position + CurrentMovementSpeed * Time.fixedDeltaTime);
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