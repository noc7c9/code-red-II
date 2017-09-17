using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class HackTimePickup : MonoBehaviour {

        public float value;

        public static event System.Action<float> PickedUpStatic;
        protected static void OnPickedUpStatic(float value) {
            var evt = PickedUpStatic;
            if (evt != null) {
                evt(value);
            }
        }

        void OnTriggerEnter(Collider col) {
            if (col.tag == "Player") {
                OnPickedUpStatic(value);
                Destroy(gameObject);
            }
        }

    }

}
