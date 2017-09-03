using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Defines the player entity's behaviour.
     */
    [RequireComponent (typeof (Rigidbody))]
    public class PlayerController : LivingEntity {

        public float moveSpeed;

        public int ammoCount;

        Vector3 velocity;
        Rigidbody myRigidbody;

        void Awake() {
            myRigidbody = GetComponent<Rigidbody>();
        }

        void Update() {
            if (transform.position.y < -10) {
                Die();
            }
        }

        void FixedUpdate() {
            myRigidbody.MovePosition(myRigidbody.position
                    + velocity * Time.fixedDeltaTime);
        }

        public void Move(Vector3 direction) {
            velocity = moveSpeed * Vector3.ClampMagnitude(direction, 1f);
        }

        public void LookAt(Vector3 point) {
            // height correct the point, so that player doesn't look down
            point = new Vector3(point.x, transform.position.y, point.z);
            transform.LookAt(point);
        }

        protected override void Die() {
            AudioManager.Instance.PlaySound("Player Death", transform.position);
            base.Die();
        }

        void OnCollisionEnter(Collision col) {
            if (col.gameObject.tag == "AmmoPickup") {
                ammoCount += col.gameObject.GetComponent<AmmoPickup>().value;
            }
        }

    }

}
