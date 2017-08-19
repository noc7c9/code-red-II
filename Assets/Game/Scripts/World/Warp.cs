using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Defines the behaviour of Warp objects.
     */
    public class Warp : MonoBehaviour {

        public int target;

        void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                Debug.Log("enter player, warp to " + target);
            }
        }

    }

}
