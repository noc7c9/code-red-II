using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class SetBossRingSpawnPoints : MonoBehaviour {

        public BossRing ring;
        public BossSpawner spawner;

        void Awake() {
            ring.Initialized -= RingInitializedHandler;
            ring.Initialized += RingInitializedHandler;
        }

        void RingInitializedHandler() {
            spawner.SetSpawnPoints(ring.enemies);
        }

    }

}
