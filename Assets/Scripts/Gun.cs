using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Defines basic gun behaviour.
 */
[RequireComponent (typeof (MuzzleFlash))]
public class Gun : MonoBehaviour {

    public Projectile projectilePrefab;
    public Transform shellPrefab;
    public Transform muzzle;
    public Transform shellEjectionPoint;

    MuzzleFlash muzzleFlash;

    public float msBetweenShots;
    public float muzzleVelocity;

    float nextShotTime;

    void Start() {
        muzzleFlash = GetComponent<MuzzleFlash>();
    }

    public void Shoot() {
        if (Time.time > nextShotTime) {
            nextShotTime = Time.time + msBetweenShots / 1000;

            Projectile newProjectile = Instantiate(projectilePrefab,
                    muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.speed = muzzleVelocity;

            Instantiate(shellPrefab,
                    shellEjectionPoint.position, shellEjectionPoint.rotation);

            muzzleFlash.Activate();
        }
    }

}
