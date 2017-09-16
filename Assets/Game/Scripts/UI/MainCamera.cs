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

        public float maxOffset;

        public LayerMask layerMask;

        Vector3 smoothVel;

        Transform player;

        float height;
        Vector3 offset;

        float collisionRadius = 0.1f;

        Vector3 playerPosition;
        Vector3 cursorPosition;
        Vector3 targetPosition;

        void Awake() {
            player = GameManager.Instance.GetPlayerController().transform;
            height = transform.position.y;
            offset = transform.position - player.position;

            // if there is sphere collider get its radius
            SphereCollider col = GetComponent<SphereCollider>();
            collisionRadius = col.radius;
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerPosition, maxOffset);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(cursorPosition, 1);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerPosition, 1);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(targetPosition, 1);
        }

        void Update() {
            if (player != null) {
                playerPosition = player.position + offset;
                cursorPosition = GameManager.Instance.GetCursorPosition();

                // make sure positions used to calculate are at camera level
                playerPosition.y = height;
                cursorPosition.y = height;

                // clamp max offset of the cursor position
                cursorPosition -= playerPosition;
                cursorPosition = Vector3.ClampMagnitude(cursorPosition, maxOffset);
                cursorPosition += playerPosition;

                // calculate target position using weights
                targetPosition = playerPosition * (1 - cursorWeight)
                    + cursorPosition * cursorWeight;

                // raycast to make sure camera doesn't pass through objects
                RaycastHit hit;
                Vector3 dir = targetPosition - transform.position;
                bool isHit = Physics.Raycast(transform.position,
                        dir, out hit, dir.sqrMagnitude, layerMask);
                if (isHit) {
                    // just clamp to collision position
                    targetPosition = hit.point - dir.normalized * collisionRadius;
                    targetPosition.y = height;
                }

                // smooth translate to target position
                transform.position = Vector3.SmoothDamp(transform.position,
                        targetPosition, ref smoothVel, smoothTime, maxSpeed);
            }
        }

    }

}
