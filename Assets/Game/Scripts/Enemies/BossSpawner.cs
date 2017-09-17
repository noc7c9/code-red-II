using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class BossSpawner : MonoBehaviour {

        public Transform enemyPrefab;

        public float spawnSecondsBetween;
        public int spawnMinCount;
        public int spawnMaxCount;

        FisherYates.ShuffleList<Transform> spawnPoints;

        float nextSpawnTime;

        void Update() {
            if (Time.time > nextSpawnTime) {
                nextSpawnTime = Time.time + spawnSecondsBetween;

                int count = Random.Range(spawnMinCount, spawnMaxCount);
                for (int i = 0; i < count; i++) {
                    SpawnEnemy();
                }
            }
        }

        void SpawnEnemy() {
            Transform enemy = Instantiate(enemyPrefab);

            // spawn at a random spawn point
            if (spawnPoints != null) {
                enemy.position = spawnPoints.Next().position;
            } else {
                enemy.position = transform.position;
            }
        }

        public void SetSpawnPoints(Transform[] points) {
            if (points.Length > 0) {
                spawnPoints = new FisherYates.ShuffleList<Transform>(points);
            } else {
                spawnPoints = null;
            }
        }

    }

}
