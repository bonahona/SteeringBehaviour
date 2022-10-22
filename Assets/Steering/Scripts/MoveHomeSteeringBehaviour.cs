using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "MoveHomeBehaviour", menuName = "Steering/Move home")]
    public class MoveHomeSteeringBehaviour: SteeringBehaviourBase
    {
        [Range(0f, 5f)]
        public float Priority = 1f;

        public override void UpdateBehaviour(SteeringAgent agent, SteeringData steeringData)
        {
            steeringData.Reset();

            var delta = (agent.StartPosition - agent.transform.position);
            steeringData.MovementFromDirection(delta.normalized * Priority, 1f);
        }
    }
}