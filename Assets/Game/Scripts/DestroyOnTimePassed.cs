using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class DestroyOnTimePassed : MonoBehaviour {

        public float lifespan;

        void Start() {
            Destroy(gameObject, lifespan);
        }

    }

}
