using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* TODO: write description
     */
    [RequireComponent(typeof(Rigidbody))]
    public class AmmoPickup : MonoBehaviour {

        public float lifespan;

        public float floatHeight;
        public float floatMaxOffset;
        public float angularSpeed;
        public float verticalSpeed;

        public int value;

        Rigidbody rb;
        float randomSinOffset;

        bool floatHeightReached;

        void Awake() {
            rb = GetComponent<Rigidbody>();
            randomSinOffset = Random.value;

            Destroy(gameObject, lifespan);
        }

        void Update() {
            if (rb.IsSleeping()) {
                rb.isKinematic = true;
            }
        }

        void FixedUpdate() {
            // initially let the physics engine control
            // then manually animate
            if (rb.isKinematic) {
                Vector3 pos = transform.position;

                if (floatHeightReached) {
                    // hover in place
                    transform.RotateAround(Vector3.zero, Vector3.up,
                            Time.time * angularSpeed);
                    pos.y = floatHeight
                        + Mathf.Sin(randomSinOffset + Time.time * verticalSpeed)
                        * floatMaxOffset;
                } else {
                    // animate to floating height
                    float originalY = pos.y;
                    pos.y += (floatHeight - pos.y) / verticalSpeed;
                    floatHeightReached = Mathf.Approximately(originalY, pos.y);
                }

                transform.position = pos;
            }
        }

        public static AmmoPickup CreateNew(AmmoPickup prefab,
                Vector3 position, Vector3 velocity) {
            AmmoPickup instance = Instantiate(prefab,
                    position, Quaternion.identity);
            instance.GetComponent<Rigidbody>().velocity = velocity;
            return instance;
        }

        void OnCollisionEnter(Collision col) {
            if (col.gameObject.tag == "Player") {
                Destroy(gameObject);
            }
        }

    }

}
