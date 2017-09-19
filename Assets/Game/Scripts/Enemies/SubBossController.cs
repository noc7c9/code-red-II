using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class SubBossController : LivingEntity {

        public const int MAX_SUB_BOSSES = 3;

        static List<SubBossController> listOfSubBossControllers;
        static SubBossController[] arrOfSubBossControllers;

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

            // remove from array
            for (int i = 0; i < MAX_SUB_BOSSES; i++) {
                if (arrOfSubBossControllers[i] == this) {
                    arrOfSubBossControllers[i] = null;
                    break;
                }
            }
        }

        void Awake() {
            if (arrOfSubBossControllers == null) {
                arrOfSubBossControllers = new SubBossController[MAX_SUB_BOSSES];
                for (int i = 0; i < MAX_SUB_BOSSES; i++) {
                    arrOfSubBossControllers[i] = null;
                }
            }

            // add to empty slot in array
            for (int i = 0; i < MAX_SUB_BOSSES; i++) {
                if (arrOfSubBossControllers[i] == null) {
                    arrOfSubBossControllers[i] = this;
                    break;
                }
            }
        }

        public static float[] GetAllHealthPercentages() {
            var result = new float[MAX_SUB_BOSSES];
            if (arrOfSubBossControllers == null) {
                return result;
            }
            for (int i = 0; i < MAX_SUB_BOSSES; i++) {
                if (arrOfSubBossControllers[i] != null) {
                    result[i] = arrOfSubBossControllers[i].healthPercentage();
                }
            }
            return result;
        }

    }

}
