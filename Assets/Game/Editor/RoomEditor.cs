using UnityEngine;
using UnityEditor;

namespace Noc7c9.TheDigitalFrontier {

    /* Editor script for the RoomLoader component.
     * Adds automatic room regeneration on changing properties.
     */
    [CustomEditor (typeof (RoomLoader))]
    public class RoomEditor : Editor {

        public override void OnInspectorGUI() {
            RoomLoader room = target as RoomLoader;

            if (DrawDefaultInspector()) {
                room.Generate();
            }

            if (GUILayout.Button("Generate Room")) {
                room.Generate();
            }
        }

    }

}
