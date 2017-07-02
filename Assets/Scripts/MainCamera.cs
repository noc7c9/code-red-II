using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Behaviour of the main camera.
 */
public class MainCamera : MonoBehaviour {

    public GameObject followTarget;
    public float smoothTime;
    public float maxSpeed;

    Vector3 initialDiff;
    Vector3 smoothVel;

    void Start() {
        if (followTarget != null) {
            initialDiff = transform.position - followTarget.transform.position;
        }
    }

    void Update() {
        if (followTarget != null) {
            Vector3 targetPos = followTarget.transform.position + initialDiff;
            transform.position = Vector3.SmoothDamp(transform.position,
                    targetPos, ref smoothVel, smoothTime, maxSpeed);
        }
    }

}
