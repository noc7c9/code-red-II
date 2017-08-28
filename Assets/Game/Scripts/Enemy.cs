using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Noc7c9.TheDigitalFrontier {

    /* Defines basic enemy behaviour.
     */
    [RequireComponent (typeof (Renderer))]
    [RequireComponent (typeof (NavMeshAgent))]
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

        public float minPathSqrDistance;
        public float attackDistanceThreshold;
        public float timeBetweenAttacks;
        public float attackSpeed;
        public float attackDamage;
        public Color attackingColor;

        float pathRefreshRate = 0.25f;

        NavMeshAgent pathfinder;
        Transform target;
        LivingEntity targetEntity;
        Material sharedMaterial;
        Material material;

        Color originalColor;

        float nextAttackTime;
        float myCollisionRadius;
        float targetCollisionRadius;

        bool hasTarget;

        void Awake() {
            pathfinder = GetComponent<NavMeshAgent>();
            sharedMaterial = GetComponent<Renderer>().sharedMaterial;

            GameObject targetObject = GameObject.FindGameObjectWithTag("Player");

            if (targetObject != null) {
                hasTarget = true;

                target = targetObject.transform;
                targetEntity = target.GetComponent<LivingEntity>();

                myCollisionRadius = GetComponent<CapsuleCollider>().radius;
                targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
            } else {
                hasTarget = false;
            }
        }

        protected override void Start() {
            base.Start();

            originalColor = sharedMaterial.color;

            if (hasTarget) {
                currentState = State.Chasing;
                targetEntity.Dying += TargetDyingEventHandler;
                StartCoroutine(UpdatePath());
            } else {
                currentState = State.Idle;
            }
        }

        public void SetCharacteristics(
                float moveSpeed, float damage, float health, Color color) {
            pathfinder.speed = moveSpeed;
            attackDamage = damage;
            startingHealth = health;
            sharedMaterial.color = color;
            material = GetComponent<Renderer>().material;
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
                    float totalThreshold = attackDistanceThreshold
                        + myCollisionRadius + targetCollisionRadius;

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
            Vector3 attackPosition = target.position - dirToTarget * myCollisionRadius;

            float percent = 0;

            // stop chasing
            currentState = State.Attacking;
            pathfinder.enabled = false;

            // change color when attacking
            material.color = attackingColor;

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
            material.color = originalColor;

            // start chasing again
            currentState = State.Chasing;
            pathfinder.enabled = true;
        }

        IEnumerator UpdatePath() {
            // while alive and there is a target, move towards the target
            // if the target is close enough
            while (hasTarget && !dead) {
                if (currentState == State.Chasing) {
                    Vector3 dirToTarget = target.position - transform.position;

                    // make sure target is close enough
                    if (dirToTarget.sqrMagnitude <= minPathSqrDistance) {
                        Vector3 targetPosition = target.position
                            - dirToTarget.normalized
                            * (myCollisionRadius + targetCollisionRadius
                                    + attackDistanceThreshold / 2);
                        pathfinder.SetDestination(targetPosition);
                    }
                }

                // recalculate path at an interval for performance
                yield return new WaitForSeconds(pathRefreshRate);
            }
        }

    }

}
