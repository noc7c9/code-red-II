using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class SubBossController : LivingEntity {

        public static event System.Action DyingStatic;
        protected static void OnDyingStatic() {
            var evt = DyingStatic;
            if (evt != null) {
                evt();
            }
        }

        protected override void Die() {
            OnDyingStatic();
            base.Die();
        }

    }

}
