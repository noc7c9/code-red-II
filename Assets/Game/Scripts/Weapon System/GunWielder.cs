using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Component that allows the entity to use Gun instances.
     */
    public class GunWielder : MonoBehaviour {

        public Transform gunPosition;

        Gun equippedGun;

        public void EquipGun() {
            EquipGun(GameManager.Instance.gun);
        }

        public void EquipGun(Gun gunToEquip) {
            if (equippedGun != null) {
                Destroy(equippedGun.gameObject);
            }
            equippedGun = Instantiate(gunToEquip,
                    gunPosition.position, gunPosition.rotation) as Gun;
            equippedGun.transform.parent = gunPosition;
        }

        public void Aim(Vector3 aimPoint) {
            if (equippedGun != null) {
                equippedGun.Aim(aimPoint);
            }
        }

        public void OnTriggerHold() {
            if (equippedGun != null) {
                equippedGun.OnTriggerHold();
            }
        }

        public void OnTriggerRelease() {
            if (equippedGun != null) {
                equippedGun.OnTriggerRelease();
            }
        }

        public void Reload() {
            if (equippedGun != null) {
                equippedGun.Reload();
            }
        }

        public float GunHeight {
            get {
                return gunPosition.position.y;
            }
        }

    }

}
