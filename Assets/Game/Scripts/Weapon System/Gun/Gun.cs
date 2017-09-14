using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Defines basic gun behaviour.
     */
    [RequireComponent (typeof (MuzzleFlash))]
    public class Gun : MonoBehaviour {

        public enum FireMode {Auto, Burst, Single};

        [Header("Parts")]
        public GunBody body;
        public GunBarrel barrel;
        public Projectile projectile;

        EffectiveGunStats stats;

        [Header("Shells")]
        public Transform shellPrefab;
        public Transform shellEjectionPoint;

        [Header("Recoil Animation")]
        public float recoilMin;
        public float recoilMax;
        public float recoilRecoveryTime;

        public float recoilAngleOffsetMin;
        public float recoilAngleOffsetMax;
        public float recoilAngleAbsoluteMax;
        public float recoilAngleRecoverTime;

        [Header("Audio")]
        public AudioClip shootAudio;

        MuzzleFlash muzzleFlash;

        float nextShotTime;

        bool triggerReleasedSinceLastShot;
        int shotsRemainingInBurst;

        Vector3 recoilSmoothDampVelocity;
        float recoilRotSmoothDampVelocity;
        float recoilAngle;

        PlayerController player;

        void Awake() {
            player = GameManager.Instance.GetPlayerController();

            // instantiate body and barrel instances as children
            // and also replace the prefabs with the actual instances
            body = Instantiate(body, transform) as GunBody;
            barrel = Instantiate(barrel, transform) as GunBarrel;

            stats = new EffectiveGunStats();
            stats.Evaluate(body, barrel, projectile);
        }

        void Start() {
            muzzleFlash = GetComponent<MuzzleFlash>();
            shotsRemainingInBurst = stats.burstCount;
            nextShotTime = Time.time;
            triggerReleasedSinceLastShot = true;
        }

        void LateUpdate() {
            // animate recoil
            transform.localPosition = Vector3.SmoothDamp(
                    transform.localPosition, Vector3.zero,
                    ref recoilSmoothDampVelocity, recoilRecoveryTime);
            recoilAngle = Mathf.SmoothDamp(recoilAngle, 0,
                    ref recoilRotSmoothDampVelocity, recoilAngleRecoverTime);
            transform.localEulerAngles = transform.localEulerAngles
                + Vector3.left * recoilAngle;
        }

        void Shoot() {
            if (Time.time > nextShotTime) {
                if (stats.fireMode == FireMode.Burst) {
                    if (shotsRemainingInBurst == 0) {
                        return;
                    }
                    shotsRemainingInBurst--;
                }
                else if (stats.fireMode == FireMode.Single) {
                    if (!triggerReleasedSinceLastShot) {
                        return;
                    }
                }

                nextShotTime = Time.time + stats.msBetweenShots / 1000;

                for (int i = 0; i < stats.projectileSpawnPoints.Length; i++) {
                    if (player.ammoCount <= 0) {
                        break;
                    }
                    player.ammoCount--;

                    Projectile newProjectile = Instantiate(projectile,
                            stats.projectileSpawnPoints[i].position,
                            stats.projectileSpawnPoints[i].rotation) as Projectile;
                    newProjectile.speed = stats.projectileSpeed;
                    newProjectile.damage = stats.damage;
                }

                Instantiate(shellPrefab,
                        shellEjectionPoint.position, shellEjectionPoint.rotation);

                muzzleFlash.Activate();

                transform.localPosition -= Vector3.forward
                    * Random.Range(recoilMax, recoilMin);
                recoilAngle += Random.Range(recoilAngleOffsetMax, recoilAngleOffsetMin);
                recoilAngle = Mathf.Clamp(recoilAngle, 0, recoilAngleAbsoluteMax);

                AudioManager.Instance.PlaySound(shootAudio, transform.position);
            }
        }

        public void Aim(Vector3 aimPoint) {
            transform.LookAt(aimPoint);
        }

        public void OnTriggerHold() {
            Shoot();
            triggerReleasedSinceLastShot = false;
        }

        public void OnTriggerRelease() {
            triggerReleasedSinceLastShot = true;
            shotsRemainingInBurst = stats.burstCount;
        }

        struct EffectiveGunStats {

            public FireMode fireMode;
            public int burstCount;

            public Transform[] projectileSpawnPoints;

            public float fireRate;
            public float msBetweenShots {
                get {
                    return 1 / fireRate * 1000;
                }
            }

            public float projectileSpeed;
            public float damage;

            public void Evaluate(GunBody body, GunBarrel barrel,
                    Projectile projectile) {
                fireRate = 0;
                projectileSpeed = 0;
                damage = 0;

                EvaluateBody(body);
                EvaluateBarrel(barrel);
                EvaluateProjectile(projectile);
            }

            void EvaluateBody(GunBody body) {
                fireMode = body.fireMode;
                burstCount = body.burstCount;

                fireRate += body.fireRate;
                projectileSpeed += body.muzzleVelocity;
                damage += body.damage;
            }

            void EvaluateBarrel(GunBarrel barrel) {
                projectileSpawnPoints = barrel.projectileSpawnPoints;

                fireRate += barrel.fireRate;
                projectileSpeed += barrel.muzzleVelocity;
                damage += barrel.damage;
            }

            void EvaluateProjectile(Projectile projectile) {
                projectileSpeed += projectile.speed;
                damage += projectile.damage;
            }

        }

    }

}
