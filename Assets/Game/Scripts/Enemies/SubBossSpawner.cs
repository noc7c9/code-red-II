using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class SubBossSpawner : MonoBehaviour {

        public Rigidbody subBossProjectilePrefab;

        public Transform[] spawnPointTransforms;

        public float fireTime;

        public int maxActiveSubBosses;
        int numOfActiveSubBosses;

        FisherYates.ShuffleList<Vector3> spawnPoints;

        float projectileRadius;
        float gravity;

        void Awake() {
            projectileRadius
                = subBossProjectilePrefab.GetComponent<SphereCollider>().radius;
            gravity = Physics.gravity.magnitude;

            spawnPoints = new FisherYates.ShuffleList<Vector3>(
                    spawnPointTransforms.Length);
            foreach (Transform point in spawnPointTransforms) {
                spawnPoints.Add(point.position);
            }

            SubBossController.DyingStatic -= SubBossDyingHandler;
            SubBossController.DyingStatic += SubBossDyingHandler;
        }

        void SubBossDyingHandler() {
            numOfActiveSubBosses -= 1;
        }

        public Transform SpawnSubBoss() {
            if (numOfActiveSubBosses >= maxActiveSubBosses) {
                return null;
            }
            numOfActiveSubBosses += 1;

            Vector3 start = transform.position;
            Vector3 end = spawnPoints.Next();

            Rigidbody proj = Instantiate(subBossProjectilePrefab);
            proj.transform.position = start;
            proj.velocity = CalculateBallisticVelocity(end);

            return proj.transform;
        }

        Vector3 CalculateBallisticVelocity(Vector3 end) {
            Vector3 start = transform.position;

            // calculate horizontal velocity
            float t = fireTime;
            Vector3 dir = end - start;
            dir.y = 0;
            Vector3 velocity = dir / fireTime;

            // calculate vertical velocity
            float height = (start - end).y - 2 * projectileRadius;
            velocity.y = -(height / fireTime - 0.5f * gravity * fireTime);

            return velocity;
        }

    }

}
