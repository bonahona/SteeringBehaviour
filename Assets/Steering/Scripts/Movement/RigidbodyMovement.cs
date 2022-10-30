using UnityEngine;

namespace Fyrvall.SteeringBehaviour.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMovement : SteeringMovementBase
    {
        private Rigidbody Rigidbody;

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
            CurrentMovementSpeed = Vector3.MoveTowards(CurrentMovementSpeed, targetMovementDirection * MovementSpeed, Acceleration * Time.fixedDeltaTime);
            Rigidbody.MovePosition(transform.position + CurrentMovementSpeed * Time.fixedDeltaTime);
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