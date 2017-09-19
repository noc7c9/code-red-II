using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Noc7c9.TheDigitalFrontier {

    public class HealthBar : MonoBehaviour {

        public RectTransform bar;
        public Text text;

        public void SetHealthValue(float percentage) {
            gameObject.SetActive(true);
            bar.localScale = new Vector3(percentage, 1, 1);
        }

        public void Disable() {
            gameObject.SetActive(false);
        }

        public void SetText(string text) {
            this.text.text = text;
        }

    }

}
