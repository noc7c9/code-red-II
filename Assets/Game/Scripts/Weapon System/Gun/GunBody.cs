using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class GunBody : MonoBehaviour {

        // unique stats
        public Gun.FireMode fireMode;
        public int burstCount;

        // shared stats
        public float fireRate;
        public float muzzleVelocity;
        public float damage;

    }

}
