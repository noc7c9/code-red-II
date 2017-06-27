using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Defines basic enemy behaviour.
 */
[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity {

    enum State {
        Chasing, Attacking,
    }
    State currentState;

    public float attackDistanceThreshold;
    public float timeBetweenAttacks;
    public float attackSpeed;
    public Color attackingColor;

    float pathRefreshRate = 0.25f;

    NavMeshAgent pathfinder;
    Transform target;
    Material skinMaterial;

    Color originalColor;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    protected override void Start() {
        base.Start();

        pathfinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        currentState = State.Chasing;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

        StartCoroutine(UpdatePath());
    }

    void Update() {
        if (Time.time > nextAttackTime) {
            float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
            if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)) {
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
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

        // lunge animation
        while (percent <= 1) {
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
        while (target != null && !dead) {
            if (currentState == State.Chasing) {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
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
