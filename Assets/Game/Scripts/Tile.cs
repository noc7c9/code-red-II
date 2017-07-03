using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    [RequireComponent (typeof (Renderer))]
    public class Tile : MonoBehaviour {

        Material material;
        Color originalColor;

        void Start() {
            material = GetComponent<Renderer>().material;
            originalColor = material.color;
        }

        public IEnumerator Flash(Color flashColor, float duration, float flashSpeed) {
            float timer = 0;
            while (timer < duration) {
                material.color = Color.Lerp(originalColor, flashColor,
                        Mathf.PingPong(timer * flashSpeed, 1));

                timer += Time.deltaTime;

                yield return null;
            }
            material.color = originalColor;
        }

    }

}
