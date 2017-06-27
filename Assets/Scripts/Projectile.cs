using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Defines behaviour of projectiles.
 */
public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    public float damage;

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
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null) {
            damageableObject.TakeHit(damage, hit);
        }
        GameObject.Destroy(gameObject);
    }

}
