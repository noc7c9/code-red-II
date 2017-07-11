using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Handles grabbing input for the player character
     * and passes it off to the player controller component.
     */
    [RequireComponent (typeof (PlayerController))]
    [RequireComponent (typeof (GunWielder))]
    public class PlayerInput : MonoBehaviour {

        public Crosshairs crosshairs;

        Camera viewCamera;
        PlayerController playerController;
        GunWielder gunWielder;

        void Awake() {
            playerController = GetComponent<PlayerController>();
            gunWielder = GetComponent<GunWielder>();
            viewCamera = Camera.main;
            GameManager.Instance.GetSpawner().OnNewWave += OnNewWave;
        }

        void OnNewWave(int waveNumber) {
            gunWielder.EquipGun(waveNumber - 1);
        }

        void Update() {
            playerController.Move(GetMoveInput());

            Vector3 point = GetLookAtPoint();
            playerController.LookAt(point);
            crosshairs.transform.position = point;
            crosshairs.DetectTargets(MouseAimRay());

            Vector2 aimPoint = new Vector2(point.x, point.z);
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
            if ((aimPoint - playerPos).sqrMagnitude > 1) {
                gunWielder.Aim(point);
            }

            // left mouse button
            if (Input.GetMouseButton(0)) {
                gunWielder.OnTriggerHold();
            }
            if (Input.GetMouseButtonUp(0)) {
                gunWielder.OnTriggerRelease();
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                gunWielder.Reload();
            }
        }

        Vector3 GetMoveInput() {
            // get input axis
            Vector3 moveInput = new Vector3(
                    Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            return moveInput.normalized;
        }

        Ray MouseAimRay() {
            return viewCamera.ScreenPointToRay(Input.mousePosition);
        }

        public Vector3 GetLookAtPoint() {
            // raycast to figure out where on the ground the mouse is pointing at.
            Ray ray = MouseAimRay();
            Plane groundPlane = new Plane(Vector3.up, Vector3.up * gunWielder.GunHeight);
            float intersectDistance;

            if (groundPlane.Raycast(ray, out intersectDistance)) {
                Vector3 point = ray.GetPoint(intersectDistance);
                return point;
            }
            return Vector3.zero;
        }

    }

}
