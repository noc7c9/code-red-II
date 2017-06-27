using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;

    [HideInInspector]
    public float speed;

    void Update() {
        float moveDistance = Time.deltaTime * speed;

        CheckCollisions(moveDistance);

        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollisions(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask)) {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit) {
        GameObject.Destroy(gameObject);
    }

}
