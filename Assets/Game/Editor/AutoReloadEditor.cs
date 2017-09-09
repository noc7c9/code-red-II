using UnityEngine;
using UnityEditor;

namespace Noc7c9.TheDigitalFrontier {

    /* Editor script for both the GameManager and RoomLoader components.
     * Adds automatic room regeneration on changing properties.
     */

    public abstract class AutoReloadEditor : Editor {

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
            gm.ReloadCityBlock();
        }

    }

    [CustomEditor (typeof (RoomLoader))]
    public class AutoReloadRoomLoaderEditor : AutoReloadEditor { }

    [CustomEditor (typeof (GameManager))]
    public class AutoReloadGameManagerEditor : AutoReloadEditor { }

}
