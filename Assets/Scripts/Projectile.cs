using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Defines behaviour of projectiles.
 */
public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    public float damage;
    public float secsLifetime;

    [HideInInspector]
    public float speed;

    float overlapThreshold = 0.1f;

    void Start() {
        Destroy(gameObject, secsLifetime);

        CheckOverlapCollisions(overlapThreshold);
    }

    void Update() {
        float moveDistance = Time.deltaTime * speed;

        CheckRayCollisions(moveDistance);

        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckOverlapCollisions(float radius) {
        Collider[] collisions = Physics.OverlapSphere(transform.position,
                radius, collisionMask);
        if (collisions.Length > 0) {
            OnHitObject(collisions[0], null);
        }
    }

    void CheckRayCollisions(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask)) {
            OnHitObject(hit.collider, hit);
        }
    }

    void OnHitObject(Collider c, RaycastHit? hit) {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null) {
            if (hit.HasValue) {
                damageableObject.TakeHit(damage, hit.Value);
            } else {
                damageableObject.TakeDamage(damage);
            }
        }
        GameObject.Destroy(gameObject);
    }

}
