using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class SpawnOnDestruction : MonoBehaviour {

        public GameObject objPrefab;

        [Range(0, 1)]
        public float spawnChance = 1;

        public float lifeSpan;

        void OnDestroy() {
            if (GameManager.sceneIsUnloading) {
                return;
            }
            if (objPrefab != null && Random.value < spawnChance) {
                GameObject obj = Instantiate(objPrefab);
                obj.transform.position = transform.position;

                if (lifeSpan > 0) {
                    Destroy(obj, lifeSpan);
                }
            }
        }

    }

}
