using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles grabbing input for the player character
 * and passes it off to the player controller component.
 */
[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunWielder))]
public class PlayerInput : MonoBehaviour {

    Camera viewCamera;
    PlayerController playerController;
    GunWielder gunWielder;

    void Start() {
        playerController = GetComponent<PlayerController>();
        gunWielder = GetComponent<GunWielder>();
        viewCamera = Camera.main;
    }

    void Update() {
        playerController.Move(GetMoveInput());
        playerController.LookAt(GetLookAtPoint());

        // left mouse button
        if (Input.GetMouseButton(0)) {
            gunWielder.OnTriggerHold();
        }
        if (Input.GetMouseButtonUp(0)) {
            gunWielder.OnTriggerRelease();
        }
    }

    Vector3 GetMoveInput() {
        // get input axis
        Vector3 moveInput = new Vector3(
                Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        return moveInput.normalized;
    }

    Vector3 GetLookAtPoint() {
        // raycast to figure out where on the ground the mouse is pointing at.
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float intersectDistance;

        if (groundPlane.Raycast(ray, out intersectDistance)) {
            Vector3 point = ray.GetPoint(intersectDistance);
            return point;
        }
        return Vector3.zero;
    }

}
