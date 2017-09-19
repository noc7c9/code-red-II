using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9 {

    public class DestroyOnCollision : MonoBehaviour {

        void OnCollisionEnter(Collision col) {
            Destroy(gameObject);
        }

    }

}
