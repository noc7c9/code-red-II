using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Defines the player entity's behaviour.
     */
    [RequireComponent (typeof (Rigidbody))]
    public class PlayerController : LivingEntity {

        public Transform turret;

        public int ammoCount;

        public bool useTraditionalInput;

        [Header("Traditional Movement")]
        public float traditionalTurnSpeed;
        public float traditionalMoveSpeed;

        [Header("Intuitive Movement")]
        public float maxMovingTurnAngle;
        public float intuitiveTurnSpeed;
        public float intuitiveMoveSpeed;

        Vector3 moveDir;

        Rigidbody rb;

        void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        void OnDrawGizmos() {
            Vector3 fow = transform.forward.normalized * 5;
            UnityEngine.Debug.DrawRay(transform.position, fow);

            // show max moving turn
            UnityEngine.Debug.DrawRay(transform.position,
                    Quaternion.Euler(0,  maxMovingTurnAngle, 0) * fow);
            UnityEngine.Debug.DrawRay(transform.position,
                    Quaternion.Euler(0, -maxMovingTurnAngle, 0) * fow);

            // draw move dir
            UnityEngine.Debug.DrawRay(
                    transform.position,
                    moveDir.normalized * 5);
        }

        void FixedUpdate() {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                useTraditionalInput = !useTraditionalInput;
            }
            if (useTraditionalInput) {
                TraditionalMovement();
            } else {
                MoveInInputDir();
            }
        }

        void TraditionalMovement() {
            float driveInput = moveDir.z;
            float turnInput = moveDir.x;

            Vector3 movement = transform.forward * driveInput
                * traditionalMoveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);

            float turnAngle = turnInput * traditionalTurnSpeed * Time.deltaTime;
            rb.MoveRotation(rb.rotation
                    * Quaternion.AngleAxis(turnAngle, Vector3.up));
        }

        void MoveInInputDir() {
            if (moveDir.sqrMagnitude <= 0) {
                return;
            }

            Vector3 forward = transform.forward;

            // move backwards if the angle is greater than 90 degrees
            if (Vector3.Angle(forward, moveDir) > 90) {
                forward = -forward;
            }

            float angle = Vector3.SignedAngle(forward, moveDir, Vector3.up);
            float angleSign = Mathf.Sign(angle);
            angle = Mathf.Abs(angle);

            if (angle <= maxMovingTurnAngle) {
                // moving
                Vector3 movement = forward * intuitiveMoveSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + movement);
            }

            // rotation
            float turn = angleSign * intuitiveTurnSpeed * Time.deltaTime;
            turn = Mathf.Clamp(turn, -angle, angle);
            Quaternion rot = Quaternion.AngleAxis(turn, Vector3.up);
            rb.MoveRotation(rb.rotation * rot);
        }

        public void Move(Vector3 direction) {
            moveDir = Vector3.ClampMagnitude(direction, 1f);
        }

        public void LookAt(Vector3 point) {
            // height correct the point, so that turret doesn't look down
            point = new Vector3(point.x, turret.transform.position.y, point.z);
            turret.transform.LookAt(point);
        }

        protected override void Die() {
            AudioManager.Instance.PlaySound("Player Death", transform.position);
            base.Die();
        }

        void OnCollisionEnter(Collision col) {
            if (col.gameObject.tag == "AmmoPickup") {
                // ammoCount += col.gameObject.GetComponent<AmmoPickup>().value;
            }
        }

    }

}
