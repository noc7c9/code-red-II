using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Defines basic gun behaviour.
 */
[RequireComponent (typeof (MuzzleFlash))]
public class Gun : MonoBehaviour {

    public enum FireMode {Auto, Burst, Single};
    public FireMode fireMode;

    public float msBetweenShots;
    public float muzzleVelocity;

    public int burstCount;

    [Header("Ammo")]
    public Projectile projectilePrefab;
    public Transform[] projectileSpawnPoints;
    public int projectilesPerMag;
    public float reloadTime;
    public float maxReloadAngle;

    [Header("Shells")]
    public Transform shellPrefab;
    public Transform shellEjectionPoint;

    [Header("Recoil")]
    public float recoilMin;
    public float recoilMax;
    public float recoilRecoveryTime;

    public float recoilAngleOffsetMin;
    public float recoilAngleOffsetMax;
    public float recoilAngleAbsoluteMax;
    public float recoilAngleRecoverTime;

    [Header("Audio")]
    public AudioClip shootAudio;
    public AudioClip reloadAudio;

    MuzzleFlash muzzleFlash;

    float nextShotTime;

    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;

    int projectilesRemainingInMag;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;

    bool isReloading;

    void Start() {
        muzzleFlash = GetComponent<MuzzleFlash>();
        shotsRemainingInBurst = burstCount;
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

        if (!isReloading && projectilesRemainingInMag == 0) {
            Reload();
        }
    }

    void Shoot() {
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0) {
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
                if (projectilesRemainingInMag == 0) {
                    break;
                }
                projectilesRemainingInMag--;
                Projectile newProjectile = Instantiate(projectilePrefab,
                        projectileSpawnPoints[i].position,
                        projectileSpawnPoints[i].rotation) as Projectile;
                newProjectile.speed = muzzleVelocity;
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

    public void Reload() {
        if (!isReloading && projectilesRemainingInMag != projectilesPerMag) {
            StartCoroutine(AnimateReload());
            AudioManager.Instance.PlaySound(reloadAudio, transform.position);
        }
    }

    IEnumerator AnimateReload() {
        isReloading = true;

        yield return new WaitForSeconds(0.2f);

        float speed = 1 / reloadTime;
        float percent = 0;
        Vector3 initialRot = transform.localEulerAngles;

        while (percent < 1) {
            percent += Time.deltaTime * speed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

            yield return null;
        }

        isReloading = false;
        projectilesRemainingInMag = projectilesPerMag;
    }

    public void Aim(Vector3 aimPoint) {
        if (!isReloading) {
            transform.LookAt(aimPoint);
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
