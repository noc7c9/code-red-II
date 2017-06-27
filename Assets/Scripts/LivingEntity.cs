using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Concrete component to allow entity to have health and be damageable.
 */
public class LivingEntity : MonoBehaviour, IDamageable {

    public float startingHealth;

    protected float health;
    protected bool dead;

    public event System.Action OnDeath;

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
        if (OnDeath != null) {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }

}
