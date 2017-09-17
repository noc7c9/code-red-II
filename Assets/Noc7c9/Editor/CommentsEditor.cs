using UnityEngine;
using UnityEditor;

namespace Noc7c9 {

    [CustomEditor (typeof (Comments))]
    public class CommentsEditor : Editor {

        public override void OnInspectorGUI() {
            Comments comments = (Comments) target;

            comments.inEditMode = GUILayout.Toggle(
                    comments.inEditMode, "Toggle Edit Mode");

            if (comments.inEditMode) {
                comments.comments = GUILayout.TextArea(comments.comments);
            } else {
                GUILayout.Label(comments.comments);
            }
        }

    }

}
