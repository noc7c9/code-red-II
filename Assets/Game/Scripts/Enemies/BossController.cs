using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class BossController : MonoBehaviour {

        public enum Stage {
            first, second, last,
        };

        public Stage stage { get; private set; }

        public BossSpawner spawner;

        public BossRing innerRing;
        public BossRing midRing;
        public BossRing outerRing;

        void Awake() {
            outerRing.Initialized += OuterRingInitializedHandler;
        }

        void OuterRingInitializedHandler() {
            spawner.SetSpawnPoints(outerRing.enemies);
        }

    }

}
