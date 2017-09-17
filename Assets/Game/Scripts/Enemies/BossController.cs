using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class BossController : MonoBehaviour {

        public enum Stage {
            FIRST, SECOND, LAST,
        };

        public enum BarrierState {
            UP, DOWN,
        }

        public BossSpawner spawner;

        public BossRing innerRing;
        public BossRing midRing;
        public BossRing outerRing;

        public Stage stage { get; private set; }

        public BarrierState barrierState { get; private set; }
        public float hackTotalTime;
        public float hackPercent {
            get {
                return 0;
            }
        }

        float timer;

        public float barrierDownDuration;

        void Awake() {
            outerRing.Initialized += OuterRingInitializedHandler;

            HackTimePickup.PickedUpStatic -= HackTimePickedupHandler;
            HackTimePickup.PickedUpStatic += HackTimePickedupHandler;
        }

        void OuterRingInitializedHandler() {
            spawner.SetSpawnPoints(outerRing.enemies);
        }

        void Update() {
            if (barrierState == BarrierState.UP) {
                timer += Time.deltaTime;
                if (timer > hackTotalTime) {
                    timer -= hackTotalTime;
                    barrierState = BarrierState.DOWN;
                }
            } else {
                timer += Time.deltaTime;
                if (timer > barrierDownDuration) {
                    timer -= barrierDownDuration;
                    barrierState = BarrierState.UP;
                }
            }
        }

        void HackTimePickedupHandler(float value) {
            if (barrierState == BarrierState.UP) {
                // bring down barrier faster
                timer += value;
            } else {
                // make barrier stay down longer
                timer -= value;
            }
        }

        public float GetHackPercentage() {
            return 100 * timer / hackTotalTime;
        }

    }

}
