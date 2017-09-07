using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class AnimateRotation : MonoBehaviour {

        public Vector3 axis;
        public float speed;

        void Update() {
            transform.Rotate(axis, speed * Time.deltaTime);
        }

    }

}
