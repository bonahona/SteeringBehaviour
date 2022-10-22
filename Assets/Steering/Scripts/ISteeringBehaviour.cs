using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public abstract class SteeringBehaviourBase : ScriptableObject
    {
        protected static readonly SteeringData SteeringDataCache = new SteeringData();

        public abstract void UpdateBehaviour(SteeringAgent agent, SteeringData steeringData);
    }
}