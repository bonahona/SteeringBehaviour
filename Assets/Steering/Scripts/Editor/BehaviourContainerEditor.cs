using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Fyrvall.SteeringBehaviour {
    [CustomEditor(typeof(BehaviourContainer))]
    public class BehaviourContainerEditor : Editor
    {
        private static List<Type> BehaviourTypes;
        private static GUIContent[] TypeNames;

        static BehaviourContainerEditor()
        {
            BehaviourTypes = typeof(BehaviourContainer).Assembly.GetTypes()
                .Where(t => typeof(SteeringBehaviourBase).IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();
            TypeNames = BehaviourTypes
                .Select(t => t.Name)
                .Select(t => new GUIContent(t))
                .ToArray();
        }

        public int SelectedIndex = 0;
        public List<Editor> SubEditors;

        private void OnEnable()
        {
            var container = target as BehaviourContainer;
            if(container?.Behaviours == null) {
                return;
            }

            SubEditors = container.Behaviours.Select(b => Editor.CreateEditor(b)).ToList();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            var container = target as BehaviourContainer;

            ListBehaviours(container);

            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope()) {
                SelectedIndex = EditorGUILayout.Popup(SelectedIndex, TypeNames);
                if(GUILayout.Button("Add")) {
                    AddBehaviour(SelectedIndex, container);
                }
            }

            if(GUILayout.Button("Update indices")) {
                container.UpdateBehaviourIndices();
                EditorUtility.SetDirty(target);
            }
        }

        private void ListBehaviours(BehaviourContainer container)
        {
            for(int i = 0; i < container.Behaviours.Count; i++) {
                var behaviour = container.Behaviours[i];
                using(new EditorGUILayout.HorizontalScope(GUI.skin.box)) {
                    using (new EditorGUILayout.VerticalScope()) {
                        SubEditors[i].OnInspectorGUI();
                    }

                    if (GUILayout.Button("Remove")) {
                        RemoveBehaviour(i, container);
                        return;
                    }
                }
            }
        }

        private void AddBehaviour(int index, BehaviourContainer container)
        {
            var behaviourType = BehaviourTypes[index];
            var behaviour = ScriptableObject.CreateInstance(behaviourType) as SteeringBehaviourBase;
            behaviour.name = behaviourType.Name;
            container.Behaviours.Add(behaviour);

            container.UpdateBehaviourIndices();

            SubEditors.Add(Editor.CreateEditor(behaviour));
            AssetDatabase.AddObjectToAsset(behaviour, target);

            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        private void RemoveBehaviour(int index, BehaviourContainer container)
        {
            GameObject.DestroyImmediate(SubEditors[index]);
            SubEditors.RemoveAt(index);

            var behaviour = container.Behaviours[index];
            AssetDatabase.RemoveObjectFromAsset(behaviour);
            container.Behaviours.RemoveAt(index);

            container.UpdateBehaviourIndices();

            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
    }
}