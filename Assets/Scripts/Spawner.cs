using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles creating enemy instances.
 */
public class Spawner : MonoBehaviour {

    public Wave[] waves;
    public Enemy enemyPrefab;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    void Start() {
        NextWave();
    }

    void Update() {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.secsBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(enemyPrefab,
                    Vector3.zero, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath() {
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0) {
            NextWave();
        }
    }

    void NextWave() {
        currentWaveNumber++;
        if (currentWaveNumber - 1 < waves.Length) {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }

    [System.Serializable]
    public class Wave {
        public int enemyCount;
        public float secsBetweenSpawns;
    }

}
