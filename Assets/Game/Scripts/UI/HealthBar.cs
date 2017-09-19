using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class HealthBar : MonoBehaviour {

        public RectTransform bar;

        public void SetHealthValue(float percentage) {
            gameObject.SetActive(true);
            bar.localScale = new Vector3(percentage, 1, 1);
        }

        public void Disable() {
            gameObject.SetActive(false);
        }

    }

}
