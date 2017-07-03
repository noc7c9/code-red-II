using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Noc7c9.TheDigitalFrontier {

    /* Editor script for the MapGenerator component.
     * Adds automatic map regeneration on changing properties.
     */
    [CustomEditor (typeof (MapGenerator))]
    public class MapEditor : Editor {

        public override void OnInspectorGUI() {
            MapGenerator map = target as MapGenerator;

            if (DrawDefaultInspector()) {
                map.GenerateMap();
            }

            if (GUILayout.Button("Generate Map")) {
                map.GenerateMap();
            }
        }

    }

}
