using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public abstract class SteeringBehaviourBase : ScriptableObject
    {
        protected static readonly SteeringData SteeringDataCache = new SteeringData();

        public abstract SteeringData UpdateBehaviour(SteeringAgent agent);
    }
}