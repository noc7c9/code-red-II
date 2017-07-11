using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Behaviour of the main camera.
     */
    public class MainCamera : MonoBehaviour {

        [Range(0, 1)]
        public float cursorWeight;

        public float smoothTime;
        public float maxSpeed;

        Vector3 offset;
        Vector3 smoothVel;

        Transform player;

        void Awake() {
            player = GameManager.Instance.GetPlayerController().transform;
        }

        void Start() {
            if (player != null) {
                offset = transform.position - player.position;
            }
        }

        void Update() {
            if (player != null) {
                Vector3 targetPosition = offset;
                targetPosition += player.position * (1 - cursorWeight);
                targetPosition += GameManager.Instance.GetCursorPosition() * cursorWeight;

                transform.position = Vector3.SmoothDamp(transform.position,
                        targetPosition, ref smoothVel, smoothTime, maxSpeed);
            }
        }

    }

}
