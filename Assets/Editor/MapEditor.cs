using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/* Editor script for the MapGenerator component.
 * Adds automatic map regeneration on changing properties.
 */
[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI() {
        MapGenerator map = target as MapGenerator;

        if (DrawDefaultInspector()) {
            base.OnInspectorGUI();

            map.GenerateMap();
        }

        if (GUILayout.Button("Generate Map")) {
            map.GenerateMap();
        }
    }

}
