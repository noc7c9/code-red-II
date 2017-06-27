using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Component that allows the entity to use Gun instances.
 */
public class GunWielder : MonoBehaviour {

    public Gun startingGun;
    public Transform gunPosition;

    Gun equippedGun;

    void Start() {
        if (startingGun != null) {
            EquipGun(startingGun);
        }
    }

    public void EquipGun(Gun gunToEquip) {
        if (equippedGun != null) {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip,
                gunPosition.position, gunPosition.rotation) as Gun;
        equippedGun.transform.parent = gunPosition;
    }

    public void Shoot() {
        if (equippedGun != null) {
            equippedGun.Shoot();
        }
    }

}
