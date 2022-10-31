using Fyrvall.SteeringBehaviour.Data;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    public abstract class SteeringBehaviourBase : ScriptableObject
    {
        protected static readonly SteeringData SteeringDataCache = new SteeringData();
        protected static readonly SteeringData WorkCache = new SteeringData();

        public bool Enabled = true;

        [Range(0f, 5f)]
        public float MovementPriority = 1f;

        [Range(0f, 5f)]
        public float OrientationPriority = 1f;

        [HideInInspector]
        public int Index;

        public virtual void StartBehaviour(SteeringAgent agent) { }
        public abstract SteeringData UpdateBehaviour(SteeringAgent agent, float deltaTime);

        public virtual void DebugDraw(SteeringAgent agent) { }
    }
}