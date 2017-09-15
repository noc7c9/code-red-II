using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Defines behaviour of explosive projectiles.
     */
    public class ExplosiveProjectile : Projectile {

        public float explosionRadius;

        void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        protected override void OnHitObject(Collider _, Vector3 hitPoint) {
            // get all objects within the explosive radius
            Collider[] collisions = Physics.OverlapSphere(hitPoint,
                    explosionRadius, collisionMask);

            // damage every object within the explosion radius
            for (int i = 0; i < collisions.Length; i++) {
                Collider c = collisions[i];
                IDamageable damageableObject = c.GetComponent<IDamageable>();
                if (damageableObject != null) {
                    Vector3 colPosition = c.transform.position;
                    Vector3 hitDirection = c.transform.position - transform.position;
                    damageableObject.TakeHit(damage, colPosition, hitDirection);

                    // UnityEngine.Debug.DrawRay(colPosition,
                    //         hitDirection.normalized * 7.5f, Color.yellow, 3);
                    // Debug.Log("Damaging", damageableObject);
                }
            }

            // before destroying the object set the position to be the hit point
            transform.position = hitPoint;

            DestroySelf();
        }

    }

}
