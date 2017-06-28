using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Defines basic enemy behaviour.
 */
[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity {

    enum State {
        Idle, Chasing, Attacking,
    }
    State currentState;

    public ParticleSystem deathEffect;

    public float attackDistanceThreshold;
    public float timeBetweenAttacks;
    public float attackSpeed;
    public float attackDamage;
    public Color attackingColor;

    float pathRefreshRate = 0.25f;

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;

    Color originalColor;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

    protected override void Start() {
        base.Start();

        pathfinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        GameObject targetObject = GameObject.FindGameObjectWithTag("Player");

        if (targetObject != null) {
            currentState = State.Chasing;
            hasTarget = true;

            target = targetObject.transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        } else {
            currentState = State.Idle;
            hasTarget = false;
        }
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection) {
        if (damage >= health) {
            Quaternion rotation =
                Quaternion.FromToRotation(Vector3.forward, hitDirection);
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, rotation)
                    as GameObject, deathEffect.main.startLifetime.constant);
        }
        base.TakeHit(damage, hitPoint, hitDirection);
    }

    void OnTargetDeath() {
        hasTarget = false;
        currentState = State.Idle;
    }

    void Update() {
        if (hasTarget) {
            if (Time.time > nextAttackTime) {
                float sqrDstToTarget = (target.position - transform.position)
                    .sqrMagnitude;
                float totalThreshold = attackDistanceThreshold
                    + myCollisionRadius + targetCollisionRadius;

                if (sqrDstToTarget < Mathf.Pow(totalThreshold, 2)) {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator Attack() {
        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * myCollisionRadius;

        float percent = 0;

        // stop chasing
        currentState = State.Attacking;
        pathfinder.enabled = false;

        // change color when attacking
        skinMaterial.color = attackingColor;

        bool hasAppliedDamage = false;

        // lunge animation and apply damage to player
        while (percent <= 1) {
            if (!hasAppliedDamage && percent >= 0.5f) {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(attackDamage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(
                    originalPosition, attackPosition, interpolation);

            yield return null;
        }

        // restore original color
        skinMaterial.color = originalColor;

        // start chasing again
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath() {
        // while alive and there is a target, move towards the target
        while (hasTarget && !dead) {
            if (currentState == State.Chasing) {
                Vector3 dirToTarget = (target.position - transform.position)
                    .normalized;
                Vector3 targetPosition = target.position
                    - dirToTarget * (myCollisionRadius + targetCollisionRadius
                            + attackDistanceThreshold / 2);
                pathfinder.SetDestination(targetPosition);
            }

            // recalculate path at an interval for performance
            yield return new WaitForSeconds(pathRefreshRate);
        }
    }

}
