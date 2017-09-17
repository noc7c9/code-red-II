using UnityEngine;

namespace Noc7c9 {

    public class Comments : MonoBehaviour {

        public string comments = "No comment.";
        public bool inEditMode;

        public void ToggleEditMode() {
            inEditMode = !inEditMode;
        }

    }

}
