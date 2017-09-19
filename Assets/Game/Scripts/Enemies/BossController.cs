using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class BossController : LivingEntity {

        public enum BarrierState {
            UP, DOWN,
        }

        public BossSpawner spawner;
        public SubBossSpawner subBossSpawner;

        public float subBossSpawnDelay;

        float subBossSpawnTimer;

        public BossRing innerRing;
        public BossRing midRing;
        public BossRing outerRing;

        public BarrierState barrierState { get; private set; }
        public float hackTotalTime;

        float timer;

        public float barrierDownDuration;

        Rigidbody rb;
        RiseAnimation rise;

        void Awake() {
            rb = GetComponent<Rigidbody>();
            rise = GetComponent<RiseAnimation>();

            HackTimePickup.PickedUpStatic -= HackTimePickedupHandler;
            HackTimePickup.PickedUpStatic += HackTimePickedupHandler;
        }

        protected override void Start() {
            base.Start();

            subBossSpawner.SpawnSubBoss();
            subBossSpawner.SpawnSubBoss();
            subBossSpawnTimer = subBossSpawnDelay / 2;
        }

        void Update() {
            if (barrierState == BarrierState.UP) {
                timer += Time.deltaTime;
                if (timer > hackTotalTime) {
                    timer -= hackTotalTime;
                    DeactivateBarrier();
                }
            } else {
                timer += Time.deltaTime;
                if (timer > barrierDownDuration) {
                    timer -= barrierDownDuration;
                    ActivateBarrier();
                }
            }

            subBossSpawnTimer += Time.deltaTime;
            if (subBossSpawnTimer > subBossSpawnDelay) {
                subBossSpawnTimer -= subBossSpawnDelay;
                subBossSpawner.SpawnSubBoss();
            }
        }

        void DeactivateBarrier() {
            barrierState = BarrierState.DOWN;

            spawner.enabled = false;

            rb.isKinematic = false;

            innerRing.ExplodeAnimation();
            midRing.ExplodeAnimation();
            outerRing.ExplodeAnimation();
        }

        void ActivateBarrier() {
            barrierState = BarrierState.UP;

            spawner.enabled = true;

            rb.isKinematic = true;
            rise.StartAnimation();

            innerRing.InitializeAnimation();
            midRing.InitializeAnimation();
            outerRing.InitializeAnimation();
        }

        void HackTimePickedupHandler(float value) {
            if (barrierState == BarrierState.UP) {
                // bring down barrier faster
                timer += value;
            } else {
                // make barrier stay down longer
                timer -= value;
                if (timer < 0) {
                    timer = 0;
                }
            }
        }

        public float GetHackPercentage() {
            return 100 * timer / hackTotalTime;
        }


        public override void TakeDamage(float damage) {
            if (barrierState == BarrierState.UP) {
                return;
            }

            base.TakeDamage(damage);
        }

    }

}
