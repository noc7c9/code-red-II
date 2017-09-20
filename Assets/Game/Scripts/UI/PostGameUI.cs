using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Noc7c9.TheDigitalFrontier {

    public class PostGameUI : MonoBehaviour {

        public float fadeInAnimationTime;

        public Text youText;
        public Text winText;

        public float youDelay;
        public float winDelay;
        public float endDelay;

        void Start() {
            StartCoroutine(Delay(youDelay, FadeInTextAnimation(youText)));
            StartCoroutine(Delay(winDelay, FadeInTextAnimation(winText)));
            StartCoroutine(Delay(endDelay, End()));
        }

        IEnumerator End() {
            SceneManager.LoadScene("Menu");
            yield return null;
        }

        IEnumerator Delay(float time, IEnumerator next) {
            yield return new WaitForSeconds(time);
            yield return next;
        }

        IEnumerator FadeInTextAnimation(Text text) {
            float speed = 1 / fadeInAnimationTime;

            float t = 0;
            Color color = text.color;
            while (t < 1) {
                t += Time.deltaTime * speed;

                color.a = Mathf.Lerp(0, 1, t);
                text.color = color;

                yield return null;
            }
        }

    }

}
