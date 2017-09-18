using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class FollowTarget : MonoBehaviour {

        public Transform target;

        void Update() {
            if (target) {
                transform.position = target.position;
            }
        }

    }

}
