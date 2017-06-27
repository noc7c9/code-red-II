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
        base.OnInspectorGUI();

        MapGenerator map = target as MapGenerator;

        map.GenerateMap();
    }

}
