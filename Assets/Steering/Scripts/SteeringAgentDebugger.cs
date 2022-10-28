using Fyrvall.SteeringBehaviour.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [RequireComponent(typeof(SteeringAgent))]
    public class SteeringAgentDebugger : MonoBehaviour
    {
        public const int FramesToKeep = 60;

        public List<SteeringFrameData> FrameData;
        [HideInInspector]
        public int CurrentIndex = 0;

        private SteeringAgent SteeringAgent;

        private void Start()
        {
            SteeringAgent = GetComponent<SteeringAgent>();
            FrameData = new List<SteeringFrameData>(FramesToKeep);
            for(int i = 0; i < FramesToKeep; i ++) {
                FrameData.Add(new SteeringFrameData(SteeringAgent.Behaviour));
            }
        }

        private void FixedUpdate()
        {
            SteeringAgent.CopyFrameData(FrameData[CurrentIndex]);

            CurrentIndex = (int)Mathf.Repeat(CurrentIndex + 1, FramesToKeep);
        }
    }
}