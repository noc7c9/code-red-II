using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles grabbing input for the player character
 * and passes it off to the player controller component.
 */
[RequireComponent (typeof (PlayerController))]
public class PlayerInput : MonoBehaviour {

    Camera viewCamera;
    PlayerController controller;

    void Start() {
        controller = GetComponent<PlayerController>();
        viewCamera = Camera.main;
    }

    void Update() {
        controller.Move(GetMoveInput());

        controller.LookAt(GetLookAtPoint());
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
