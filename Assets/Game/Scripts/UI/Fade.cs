using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Noc7c9.TheDigitalFrontier {

    [RequireComponent(typeof(Image))]
    public class Fade : MonoBehaviour {

        public Color color;

        Image image;

        void Awake() {
            image = GetComponent<Image>();
        }

        public void SetColor(Color newColor) {
            newColor.a = color.a;
            color = newColor;
        }

        public void SetOpacity(float value) {
            color.a = value;
            image.color = color;
        }

        IEnumerator currentAnimation;

        public void Animate(float opacity, float time) {
            if (currentAnimation != null) {
                StopCoroutine(currentAnimation);
            }
            currentAnimation = Animation(opacity, time);
            StartCoroutine(currentAnimation);
        }

        IEnumerator Animation(float finalOpacity, float time) {
            float start = image.color.a;
            float end = finalOpacity;

            float speed = 1 / time;

            float t = 0;
            while (t < 1) {
                t += Time.deltaTime * speed;

                color.a = Mathf.Lerp(start, end, t);
                image.color = color;

                yield return null;
            }

            currentAnimation = null;
        }

    }

}
