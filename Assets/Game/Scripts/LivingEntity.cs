using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Concrete component to allow entity to have health and be damageable.
     */
    public class LivingEntity : MonoBehaviour, IDamageable {

        public float startingHealth;

        public float health { get; protected set; }
        protected bool dead;

        public event System.Action OnDeath;

        protected virtual void Start() {
            health = startingHealth;
        }

        public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection) {
            TakeDamage(damage);
        }

        public virtual void TakeDamage(float damage) {
            health -= damage;

            if (health <= 0 && !dead) {
                Die();
            }
        }

        protected virtual void Die() {
            if (!dead) {
                dead = true;
                health = 0;
                if (OnDeath != null) {
                    OnDeath();
                }
                GameObject.Destroy(gameObject);
            }
        }

    }

}
