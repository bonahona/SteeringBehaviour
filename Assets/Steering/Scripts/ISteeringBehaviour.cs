using Fyrvall.SteeringBehaviour.Data;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public abstract class SteeringBehaviourBase : ScriptableObject
    {
        protected static readonly SteeringData SteeringDataCache = new SteeringData();
        protected static readonly SteeringData WorkCache = new SteeringData();

        public bool Enabled = true;

        [HideInInspector]
        public int Index;

        public virtual void StartBehaviour(SteeringAgent agent) { }
        public abstract SteeringData UpdateBehaviour(SteeringAgent agent, float deltaTime);

        public virtual void DebugDraw(SteeringAgent agent) { }
    }
}