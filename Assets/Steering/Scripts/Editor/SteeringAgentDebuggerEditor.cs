using Fyrvall.SteeringBehaviour.Data;
using UnityEditor;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CustomEditor(typeof(SteeringAgentDebugger))]
    public class SteeringAgentDebuggerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if(!Application.isPlaying) {
                EditorGUILayout.HelpBox("Application not running", MessageType.Warning);
                return;
            }

            var agentDebugger = (SteeringAgentDebugger)target;

            foreach(var frameData in agentDebugger.FrameData) {
                ShowFrameData(frameData);
                EditorGUILayout.Space();
            }
        }

        private void ShowFrameData(SteeringFrameData frameData)
        {
            using(new EditorGUILayout.VerticalScope(GUI.skin.box)) {
                foreach(var buffer in frameData.FrameDataBuffer) {
                    using (new EditorGUILayout.HorizontalScope()) {
                        EditorGUILayout.LabelField(buffer.Behaviour);
                        foreach(var direction in buffer.SteeringData.Directions) {
                            EditorGUILayout.LabelField(direction.MovementWeight.ToString());
                        }
                    }
                }
            }
        }
    }
}