using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class BossSpawner : MonoBehaviour {

        public Transform enemyPrefab;
        public float spawnTime;

        float nextSpawnTime;

        void Update() {
            if (Time.time > nextSpawnTime) {
                nextSpawnTime = Time.time + spawnTime;
                SpawnEnemy();
            }
        }

        void SpawnEnemy() {
            Transform enemy = Instantiate(enemyPrefab);
            enemy.position = transform.position;
        }

    }

}
