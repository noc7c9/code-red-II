using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Gun muzzle flash
     */
    public class MuzzleFlash : MonoBehaviour {

        public GameObject flashHolder;
        public Sprite[] flashSprites;
        public SpriteRenderer[] spriteRenderers;

        public float flashTime;

        void Start() {
            Deactivate();
        }

        public void Activate() {
            int flashSpriteIndex = Random.Range(0, flashSprites.Length);
            for (int i = 0; i < spriteRenderers.Length; i++) {
                spriteRenderers[i].sprite = flashSprites[flashSpriteIndex];
            }

            flashHolder.SetActive(true);

            Invoke("Deactivate", flashTime);
        }

        void Deactivate() {
            flashHolder.SetActive(false);
        }

    }

}
