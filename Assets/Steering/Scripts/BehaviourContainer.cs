using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "BehaviourContainer", menuName = "Steering/Container")]
    public class BehaviourContainer : ScriptableObject
    {
        [Range(0f, 5f)]
        public float ClampMovement = 0.1f;

        [HideInInspector]
        public List<SteeringBehaviourBase> Behaviours = new List<SteeringBehaviourBase>();
    }
}