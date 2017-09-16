using UnityEngine;

namespace Noc7c9 {

    static public class TransformExtensions {

        static public void DestroyChildren(this Transform parent, float time=0) {
            int childCount = parent.childCount;
            for (int i = childCount - 1; i >= 0; i--) {
                GameObject.Destroy(parent.GetChild(i).gameObject, time);
            }
        }

        static public void DestroyImmediateChildren(this Transform parent) {
            int childCount = parent.childCount;
            for (int i = childCount - 1; i >= 0; i--) {
                GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }

    }

}
