using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9 {

    public class DebugCamera : MonoBehaviour {

        float epsilon = 0.01f;

        [Header("Reset Position Button")]
        public KeyCode resetKey = KeyCode.R;

        [Header("WASD Movement")]
        public bool wasdMovementEnabled = true;

        public Vector3 wsAxis = Vector3.forward;
        public Vector3 adAxis = Vector3.left;

        public bool useConstantSpeed = false;
        public float constantSpeed = 20;

        public float acceleration = 5;
        public float maxVelocity = 20;
        [Range(1, 1000)]
        public float frictionCoefficent = 100;

        [Header("MouseWheel Movement")]
        public bool mouseWheelMovementEnabled = true;

        public Vector3 wheelAxis = Vector3.down;
        public float speed = 10;

        Vector3 initialPosition;

        Vector3 velocity = Vector3.zero;

        void Start() {
            initialPosition = transform.position;
        }

        void Update() {
            resetPosition();

            if (wasdMovementEnabled) {
                WASDMovement();
            }

            if (mouseWheelMovementEnabled) {
                MouseWheelMovement();
            }
        }

        void resetPosition() {
            if (Input.GetKeyDown(resetKey)) {
                transform.position = initialPosition;

                velocity = Vector3.zero;
            }
        }

        void WASDMovement() {
            float WSInput = 0
                + (Input.GetKey(KeyCode.W) ? 1 : 0)
                + (Input.GetKey(KeyCode.S) ? -1 : 0);
            float ADInput = 0
                + (Input.GetKey(KeyCode.A) ? 1 : 0)
                + (Input.GetKey(KeyCode.D) ? -1 : 0);

            if (useConstantSpeed) {
                velocity = Vector3.zero
                    + wsAxis.normalized * WSInput
                    + adAxis.normalized * ADInput;
                velocity = velocity.normalized * constantSpeed;
            } else {
                Vector3 dir = wsAxis.normalized * WSInput
                    + adAxis.normalized * ADInput;

                // add accel
                velocity += dir.normalized * acceleration;

                // apply friction
                velocity -= velocity * frictionCoefficent / 1000;
                // allow friction to stop the camera
                if (velocity.sqrMagnitude < epsilon) {
                    velocity = Vector3.zero;
                }

                // clamp to max velocity
                velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
            }

            transform.position += velocity * Time.deltaTime;
        }

        void MouseWheelMovement() {
            float input = Input.mouseScrollDelta.y;
            transform.position += wheelAxis * input * speed * Time.deltaTime;
        }

    }

}
