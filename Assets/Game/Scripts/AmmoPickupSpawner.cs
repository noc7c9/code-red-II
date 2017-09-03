using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Spawns ammo pickups when particles collide with things
     */
    [RequireComponent(typeof (ParticleSystem))]
    public class AmmoPickupSpawner : MonoBehaviour {

        public AmmoPickup ammoPickupPrefab;

        public int maxDrops;
        public int minDrops;
        int numberOfDrops;

        List<ParticleCollisionEvent> collisionEvents;
        ParticleSystem particleSys;

        void Start() {
            particleSys = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();

            numberOfDrops = Random.Range(minDrops, maxDrops);
        }

        void OnParticleCollision(GameObject other) {
            particleSys.GetCollisionEvents(other, collisionEvents);

            foreach (ParticleCollisionEvent col in collisionEvents) {
                if (numberOfDrops > 0) {
                    AmmoPickup.CreateNew(ammoPickupPrefab,
                            col.intersection, col.velocity);
                }
                numberOfDrops -= 1;
            }
        }

    }

}
