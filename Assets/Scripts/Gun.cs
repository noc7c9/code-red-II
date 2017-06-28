using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Defines basic gun behaviour.
 */
[RequireComponent (typeof (MuzzleFlash))]
public class Gun : MonoBehaviour {

    public enum FireMode {Auto, Burst, Single};
    public FireMode fireMode;

    public Projectile projectilePrefab;
    public Transform shellPrefab;
    public Transform[] projectileSpawnPoints;
    public Transform shellEjectionPoint;

    public float msBetweenShots;
    public float muzzleVelocity;

    public int burstCount;

    MuzzleFlash muzzleFlash;

    float nextShotTime;

    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;

    void Start() {
        muzzleFlash = GetComponent<MuzzleFlash>();
        shotsRemainingInBurst = burstCount;
    }

    void Shoot() {
        if (Time.time > nextShotTime) {
            if (fireMode == FireMode.Burst) {
                if (shotsRemainingInBurst == 0) {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single) {
                if (!triggerReleasedSinceLastShot) {
                    return;
                }
            }

            nextShotTime = Time.time + msBetweenShots / 1000;

            for (int i = 0; i < projectileSpawnPoints.Length; i++) {
                Projectile newProjectile = Instantiate(projectilePrefab,
                        projectileSpawnPoints[i].position,
                        projectileSpawnPoints[i].rotation) as Projectile;
                newProjectile.speed = muzzleVelocity;
            }

            Instantiate(shellPrefab,
                    shellEjectionPoint.position, shellEjectionPoint.rotation);

            muzzleFlash.Activate();
        }
    }

    public void OnTriggerHold() {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease() {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }

}
