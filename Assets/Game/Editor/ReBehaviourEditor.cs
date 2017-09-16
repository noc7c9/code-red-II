using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Noc7c9.TheDigitalFrontier {

    [CustomEditor (typeof (MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public class ReBehaviourEditor : Editor {

        bool autoReAwake;
        bool autoReStart;

        public override void OnInspectorGUI() {
            bool changed = DrawDefaultInspector();

            if (Application.isPlaying) {
                MonoBehaviour obj = (MonoBehaviour)target;

                if (changed && autoReAwake) {
                    ReAwake();
                }
                if (changed && autoReStart) {
                    ReStart();
                }

                GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                        GUILayout.Label("Re-Awake");
                        GUILayout.Label("Re-Start");
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                        GUILayout.BeginHorizontal();
                            if (GUILayout.Button("Run")) {
                                ReAwake();
                            }
                            autoReAwake = GUILayout.Toggle(autoReAwake, "Auto");
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                            if (GUILayout.Button("Run")) {
                                ReStart();
                            }
                            autoReStart = GUILayout.Toggle(autoReStart, "Auto");
                        GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        void ReAwake() {
            for (int i = 0; i < targets.Length; i++) {
                MonoBehaviour obj = (MonoBehaviour)targets[i];
                obj.SendMessage("Awake");
            }
        }

        void ReStart() {
            for (int i = 0; i < targets.Length; i++) {
                MonoBehaviour obj = (MonoBehaviour)targets[i];
                obj.SendMessage("Start");
            }
        }

    }

}
