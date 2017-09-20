using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Noc7c9.TheDigitalFrontier {

    public class HoverOutline : MonoBehaviour {

        public Outline outline;

        [Range(0, 255)]
        public float normalOpacity;
        float normalOpacityPercent {
            get {
                return normalOpacity / 255f;
            }
        }

        [Range(0, 255)]
        public float hoverOpacity;
        float hoverOpacityPercent {
            get {
                return hoverOpacity / 255f;
            }
        }

        public float animationSpeed;

        void Awake() {
            Color c = outline.effectColor;
            c.a = normalOpacityPercent;
            outline.effectColor = c;
        }

        public void OnPointerEnter() {
            StartAnimation(hoverOpacityPercent);
        }

        public void OnPointerExit() {
            StartAnimation(normalOpacityPercent);
        }

        void StartAnimation(float finalOpacity) {
            if (currentAnimation != null) {
                StopCoroutine(currentAnimation);
            }

            currentAnimation = AnimateColor(finalOpacity);
            StartCoroutine(currentAnimation);
        }

        IEnumerator currentAnimation;
        IEnumerator AnimateColor(float finalOpacity) {
            float start = outline.effectColor.a;
            float end = finalOpacity;

            float t = 0;
            Color c = outline.effectColor;
            while (t < 1) {
                t += Time.deltaTime * animationSpeed;

                c.a = Mathf.Lerp(start, end, t);
                outline.effectColor = c;

                yield return null;
            }

            currentAnimation = null;
        }

    }

}
