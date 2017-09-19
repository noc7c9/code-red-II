using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class SubBossSpawner : MonoBehaviour {

        public Rigidbody subBossProjectilePrefab;

        public Transform[] spawnPointTransforms;

        public float fireTime;

        public int numOfActiveSubBosses { get; private set; }

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
            if (numOfActiveSubBosses >= SubBossController.MAX_SUB_BOSSES) {
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
