using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class AnimateRotation : MonoBehaviour {

        public bool usePerlinMovingAxis;
        public Vector3 offset;
        public float scale;

        public Vector3 axis;
        public float speed;

        public bool drawGizmos;

        void OnDrawGizmos() {
            if (drawGizmos) {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, axis.normalized * 10);
            }
        }

        void Update() {
            if (usePerlinMovingAxis) {
                axis = PerlinNoiseVector3(offset, scale);
            }
            transform.Rotate(axis, speed * Time.deltaTime);
        }

        static Vector3 PerlinNoiseVector3(Vector3 offset, float scale) {
            float off = Time.time * scale;
            Debug.Log(off);
            return new Vector3(
                2 * Mathf.PerlinNoise(offset.x + off, 0) - 1,
                2 * Mathf.PerlinNoise(offset.y + off, 0) - 1,
                2 * Mathf.PerlinNoise(offset.z + off, 0) - 1
            );
        }

    }

}
