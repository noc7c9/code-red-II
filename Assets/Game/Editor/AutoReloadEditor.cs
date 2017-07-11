using UnityEngine;
using UnityEditor;

namespace Noc7c9.TheDigitalFrontier {

    /* Editor script for the RoomLoader component.
     * Adds automatic room regeneration on changing properties.
     */
    [CustomEditor (typeof (RoomLoader))]
    public class AutoReloadRoomLoaderEditor : Editor {

        public override void OnInspectorGUI() {
            if (DrawDefaultInspector()) {
                Regenerate();
            }

            if (GUILayout.Button("Generate Room")) {
                Regenerate();
            }
        }

        void Regenerate() {
            GameManager gm = GameManager.Instance;
            gm.GetRoomLoader().Load(gm.levels[gm.visibleLevel].roomSettings);
        }

    }

    [CustomEditor (typeof (GameManager))]
    public class AutoReloadGameManagerEditor : Editor {

        public override void OnInspectorGUI() {
            if (DrawDefaultInspector()) {
                Regenerate();
            }

            if (GUILayout.Button("Generate Room")) {
                Regenerate();
            }
        }

        void Regenerate() {
            GameManager gm = target as GameManager;
            gm.GetRoomLoader().Load(gm.levels[gm.visibleLevel].roomSettings);
        }

    }

}
