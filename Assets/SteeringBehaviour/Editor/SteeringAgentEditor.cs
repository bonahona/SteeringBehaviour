using UnityEditor;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SteeringAgent))]
    public class SteeringAgentEditor : Editor
    {
        public const float DirectionCircleRadius = 0.5f;

        private void OnSceneGUI()
        {
            var steeringAgent = target as SteeringAgent;

            if(!Application.isPlaying) {
                return;
            }

            var agentPosition = steeringAgent.transform.position;
            Handles.DrawWireDisc(agentPosition, Vector3.up, DirectionCircleRadius);

            foreach(var direction in steeringAgent.CurrentSteeringData.Directions) {
                var startPosition = agentPosition + direction.Direction * DirectionCircleRadius;

                if (direction.Weight > 0) {
                    Debug.DrawLine(startPosition, startPosition + direction.Direction * direction.Weight, Color.Lerp(Color.yellow, Color.green, direction.Weight));
                } else {
                    Debug.DrawLine(startPosition, startPosition + direction.Direction * Mathf.Abs(direction.Weight), Color.red);
                }
            }
        }
    }
}