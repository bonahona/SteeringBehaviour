using System.Linq;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CreateAssetMenu(fileName = "Default TargetProvider", menuName = "Target/Default Target Provider")]
    public class TargetProvider : ScriptableObject
    {
        public SteeringAgent GetTarget(SteeringAgent agent)
        {
            return GameObject.FindObjectsOfType<SteeringAgent>()
                .Where(a => a.Team != agent.Team)
                .FirstOrDefault();
        }
    }
}