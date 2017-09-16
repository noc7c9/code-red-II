using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Defines behaviour of projectiles.
     */
    [RequireComponent (typeof (TrailRenderer))]
    public class Projectile : MonoBehaviour {

        // unique stats

        // shared stats
        public float speed;
        public float damage;

        public ParticleSystem hitEffect;

        public LayerMask collisionMask;
        public Color trailColor;
        public float secsLifetime;

        public float collisionRadius;

        bool isDestroyed;

        void Start() {
            Destroy(gameObject, secsLifetime);

            CheckOverlapCollisions(collisionRadius);

            GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
        }

        void Update() {
            float moveDistance = Time.deltaTime * speed;
            CheckRayCollisions(moveDistance);

            if (!isDestroyed) {
                transform.Translate(Vector3.forward * moveDistance);
            }
        }

        void CheckOverlapCollisions(float radius) {
            Collider[] collisions = Physics.OverlapSphere(transform.position,
                    radius, collisionMask);
            if (collisions.Length > 0) {
                OnHitObject(collisions[0], transform.position);
            }
        }

        void CheckRayCollisions(float moveDistance) {
            RaycastHit hit;

            bool isHit = Physics.SphereCast(transform.position, collisionRadius,
                    transform.forward, out hit, moveDistance, collisionMask);
            // Ray ray = new Ray(transform.position, transform.forward);
            // if (Physics.Raycast(ray, out hit, moveDistance, collisionMask)) {
            if (isHit) {
                OnHitObject(hit.collider, hit.point);
            }
        }

        protected virtual void OnHitObject(Collider c, Vector3 hitPoint) {
            IDamageable damageableObject = c.GetComponent<IDamageable>();
            Vector3 hitDirection = transform.forward;

            if (damageableObject != null) {
                damageableObject.TakeHit(damage, hitPoint, hitDirection);

            } else {
                // show hit effect
                Quaternion rotation =
                    Quaternion.FromToRotation(Vector3.forward, -hitDirection);
                Destroy(Instantiate(hitEffect.gameObject, hitPoint, rotation)
                        as GameObject, hitEffect.main.startLifetime.constant);
            }

            // before destroying the object set the position to be the hit point
            transform.position = hitPoint;

            // and destroy the projectile
            DestroySelf();
        }

        protected void DestroySelf() {
            GameObject.Destroy(gameObject);
            isDestroyed = true;
        }

    }

}
