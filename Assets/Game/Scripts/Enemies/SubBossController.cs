using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class SubBossController : LivingEntity {

        public const int MAX_SUB_BOSSES = 3;

        static SubBossController[] allSubBosses_ = new SubBossController[MAX_SUB_BOSSES];
        public static SubBossController[] allSubBosses {
            get {
                return allSubBosses_;
            }
        }
        static int allSubBossCount;

        public static event System.Action DyingStatic;
        protected static void OnDyingStatic() {
            var evt = DyingStatic;
            if (evt != null) {
                evt();
            }
        }

        public int number { get; private set; }

        RiseAnimation rise;
        BossSpawner spawner;

        void Awake() {
            rise = GetComponent<RiseAnimation>();
            spawner = GetComponent<BossSpawner>();

            allSubBossCount += 1;
            number = allSubBossCount;

            // add to empty slot in array
            for (int i = 0; i < MAX_SUB_BOSSES; i++) {
                if (allSubBosses_[i] == null) {
                    allSubBosses_[i] = this;
                    break;
                }
            }
        }

        void Start() {
            spawner.enabled = false;
            rise.Ending += () => {
                spawner.enabled = true;
            };
            rise.StartAnimation();
        }

        protected override void Die() {
            OnDyingStatic();
            base.Die();

            // remove from array
            for (int i = 0; i < MAX_SUB_BOSSES; i++) {
                if (allSubBosses_[i] == this) {
                    allSubBosses_[i] = null;
                    break;
                }
            }
        }

    }

}
