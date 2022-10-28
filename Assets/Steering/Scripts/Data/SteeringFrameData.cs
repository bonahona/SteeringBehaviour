using System.Collections.Generic;

namespace Fyrvall.SteeringBehaviour.Data
{
    [System.Serializable]
    public class SteeringFrameData
    {
        public class BehaviourSteeringData
        {
            public string Behaviour;
            public SteeringData SteeringData;

            public BehaviourSteeringData(SteeringBehaviourBase steeringBehaviour)
            {
                Behaviour = steeringBehaviour.name;
                SteeringData = new SteeringData();
            }
        }

        public List<BehaviourSteeringData> FrameDataBuffer;

        public SteeringFrameData(BehaviourContainer behaviourContainer)
        {      
            if (behaviourContainer == null) {
                FrameDataBuffer = new List<BehaviourSteeringData>();
                return;
            }

            FrameDataBuffer = new List<BehaviourSteeringData>(behaviourContainer.Behaviours.Count);
            for (int i = 0; i < behaviourContainer.Behaviours.Count; i++) { 
                FrameDataBuffer.Add(new BehaviourSteeringData(behaviourContainer.Behaviours[i]));
            }
        }

        public void RegisterSteeringData(SteeringBehaviourBase behaviourBase, SteeringData data)
        {
            if(behaviourBase == null) {
                return;
            }

            FrameDataBuffer[behaviourBase.Index].SteeringData.Copy(data);
        }

        public void Copy(SteeringFrameData other)
        {
            for (int i = 0; i < FrameDataBuffer.Count; i++) {
                if (other.FrameDataBuffer[i] == null) {
                    continue;
                }

                FrameDataBuffer[i].SteeringData.Copy(other.FrameDataBuffer[i].SteeringData);
            }
        }
    }
}