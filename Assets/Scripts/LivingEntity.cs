using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Concrete component to allow entity to have health and be damageable.
 */
public class LivingEntity : MonoBehaviour, IDamageable {

    public float startingHealth;

    protected float health;
    protected bool dead;

    protected virtual void Start() {
        health = startingHealth;
    }

    public void TakeHit(float damage, RaycastHit hit) {
        health -= damage;

        if (health <= 0 && !dead) {
            Die();
        }
    }

    void Die() {
        dead = true;
        GameObject.Destroy(gameObject);
    }

}
