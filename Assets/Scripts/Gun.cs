﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Defines basic gun behaviour.
 */
public class Gun : MonoBehaviour {

    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots;
    public float muzzleVelocity;

    float nextShotTime;

    public void Shoot() {
        if (Time.time > nextShotTime) {
            nextShotTime = Time.time + msBetweenShots / 1000;

            Projectile newProjectile = Instantiate(projectile,
                    muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.speed = muzzleVelocity;
        }
    }

}
