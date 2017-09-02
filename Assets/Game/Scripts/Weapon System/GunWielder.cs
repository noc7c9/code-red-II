using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Component that allows the entity to use Gun instances.
     */
    public class GunWielder : MonoBehaviour {

        public Transform gunPosition;

        bool firstGunEquipped = true;
        Gun equippedGun;

        public event System.Action<bool> SwappedGun;
        protected virtual void OnSwappedGun(bool firstGunEquipped) {
            var evt = SwappedGun;
            if (evt != null) {
                evt(firstGunEquipped);
            }
        }

        public void SwapGun() {
            firstGunEquipped = !firstGunEquipped;
            EquipGun();
            OnSwappedGun(firstGunEquipped);
        }

        public void EquipGun() {
            EquipGun(firstGunEquipped
                    ? GameManager.Instance.gun1
                    : GameManager.Instance.gun2);
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
