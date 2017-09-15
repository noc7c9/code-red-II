using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class SpawnOnDestruction : MonoBehaviour {

        public GameObject objPrefab;

        public float lifeSpan;

        void OnDestroy() {
            if (objPrefab != null) {
                GameObject obj = Instantiate(objPrefab);
                obj.transform.position = transform.position;

                if (lifeSpan > 0) {
                    Destroy(obj, lifeSpan);
                }
            }
        }

    }

}
