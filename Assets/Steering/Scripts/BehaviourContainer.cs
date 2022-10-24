using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "BehaviourContainer", menuName = "Steering/Container")]
    public class BehaviourContainer : ScriptableObject
    {
        public List<SteeringBehaviourBase> Behaviours = new List<SteeringBehaviourBase>();
    }
}