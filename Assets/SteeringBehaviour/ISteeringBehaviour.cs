using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public abstract class SteeringBehaviourBase : ScriptableObject
    {
        public abstract void UpdateBehaviour(SteeringAgent agent, SteeringData steeringData);
    }
}