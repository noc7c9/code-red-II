using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class SpawnOnDestruction : MonoBehaviour {

        public GameObject objPrefab;

        [Range(0, 1)]
        public float spawnChance = 1;

        // can't use enabled because unity will change it when calling OnDestroy
        bool isEnabled;
        bool isQuitting;

        void Awake() {
            isEnabled = enabled;
        }

        void Start() {
            // allow component to be enabled and disabled
        }

        void OnApplicationQuit() {
            isQuitting = true;
        }

        void OnDestroy() {
            if (isQuitting || GameManager.sceneIsUnloading || !isEnabled) {
                return;
            }
            if (objPrefab != null && Random.value < spawnChance) {
                GameObject obj = Instantiate(objPrefab);
                obj.transform.position = transform.position;
            }
        }

    }

}
