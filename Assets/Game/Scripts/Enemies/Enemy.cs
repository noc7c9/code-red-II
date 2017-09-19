using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Noc7c9.TheDigitalFrontier {

    /* Defines basic enemy behaviour.
     */
    [RequireComponent (typeof (NavMeshAgent))]
    [SelectionBase]
    public class Enemy : LivingEntity {

        public static event System.Action DyingStatic;
        protected static void OnDyingStatic() {
            var d = DyingStatic;
            if (d != null) {
                d();
            }
        }

        enum State {
            Idle, Chasing, Attacking,
        }
        State currentState;

        public ParticleSystem deathEffect;

        public bool useDistanceLimit;
        public float minPathSqrDistance;

        public float moveSpeed;

        public Color originalColor;

        public float attackRange;
        public float attackMovedDistance;
        public float timeBetweenAttacks;
        public float attackSpeed;
        public float attackDamage;
        public Color attackingColor;

        float pathRefreshRate = 0.25f;

        NavMeshAgent pathfinder;
        Transform target;
        LivingEntity targetEntity;

        float nextAttackTime;

        bool hasTarget;

        void Awake() {
            pathfinder = GetComponent<NavMeshAgent>();

            GameObject targetObject = GameObject.FindGameObjectWithTag("Player");

            if (targetObject != null) {
                hasTarget = true;

                target = targetObject.transform;
                targetEntity = target.GetComponent<LivingEntity>();
            } else {
                hasTarget = false;
            }
        }

        protected override void Start() {
            base.Start();

            pathfinder.speed = moveSpeed;

            if (hasTarget) {
                currentState = State.Chasing;
                targetEntity.Dying += TargetDyingEventHandler;
                StartCoroutine(UpdatePath());
            } else {
                currentState = State.Idle;
            }
        }

        public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection) {
            AudioManager.Instance.PlaySound("Impact", transform.position);
            if (damage >= health && !dead) {
                OnDyingStatic();

                AudioManager.Instance.PlaySound("Enemy Death", transform.position);
                Quaternion rotation =
                    Quaternion.FromToRotation(Vector3.forward, hitDirection);
                Destroy(Instantiate(deathEffect.gameObject, hitPoint, rotation)
                        as GameObject, deathEffect.main.startLifetime.constant);
            }
            base.TakeHit(damage, hitPoint, hitDirection);
        }

        void TargetDyingEventHandler() {
            hasTarget = false;
            currentState = State.Idle;
        }

        void Update() {
            if (hasTarget) {
                if (Time.time > nextAttackTime) {
                    float sqrDstToTarget = (target.position - transform.position)
                        .sqrMagnitude;
                    float totalThreshold = attackRange;

                    if (sqrDstToTarget < Mathf.Pow(totalThreshold, 2)) {
                        nextAttackTime = Time.time + timeBetweenAttacks;
                        StartCoroutine(Attack());
                        AudioManager.Instance.PlaySound("Enemy Attack", transform.position);
                    }
                }
            }
        }

        IEnumerator Attack() {
            Vector3 originalPosition = transform.position;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            // Vector3 attackPosition = target.position - dirToTarget * attackMovedDistance;
            Vector3 attackPosition = originalPosition + dirToTarget * attackMovedDistance;

            float percent = 0;

            // stop chasing
            currentState = State.Attacking;
            pathfinder.enabled = false;

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

            // start chasing again
            currentState = State.Chasing;
            pathfinder.enabled = true;
        }

        IEnumerator UpdatePath() {
            // while alive and there is a target, move towards the target
            // if the target is close enough
            while (hasTarget && !dead) {
                if (currentState == State.Chasing && pathfinder.enabled) {
                    Vector3 dirToTarget = target.position - transform.position;

                    // make sure target is close enough
                    if (!useDistanceLimit ||
                            dirToTarget.sqrMagnitude <= minPathSqrDistance) {
                        Vector3 targetPosition = target.position
                            - dirToTarget.normalized * attackRange * 0.8f;
                        pathfinder.SetDestination(targetPosition);
                    }
                }

                // recalculate path at an interval for performance
                yield return new WaitForSeconds(pathRefreshRate);
            }
        }

    }

}
