using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public abstract class SteeringBehaviourBase : ScriptableObject
    {
        protected static readonly SteeringData SteeringDataCache = new SteeringData();
        protected static readonly SteeringData WorkCache = new SteeringData();

        public bool Enabled = true;

        public virtual void StartBehaviour(SteeringAgent agent) { }
        public abstract SteeringData UpdateBehaviour(SteeringAgent agent);
    }
}