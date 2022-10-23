using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public abstract class SteeringBehaviourBase : ScriptableObject
    {
        protected static readonly SteeringData SteeringDataCache = new SteeringData();
        protected static readonly SteeringData WorkCache = new SteeringData();

        public abstract SteeringData UpdateBehaviour(SteeringAgent agent);
    }
}