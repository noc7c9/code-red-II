using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Interface that allows entity to define it can be damaged.
 */
public interface IDamageable {

    void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection);
    void TakeDamage(float damage);

}
