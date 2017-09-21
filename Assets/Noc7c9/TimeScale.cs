using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9 {

    public class TimeScale : MonoBehaviour {

        public float timeScale = 1;

        void Update() {
            Time.timeScale = timeScale;
        }

        void OnDisable() {
            Time.timeScale = 1;
        }

        void OnValidate() {
            if (timeScale < 0) {
                timeScale = 0;
            }
        }

    }

}
