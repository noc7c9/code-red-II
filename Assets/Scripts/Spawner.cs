﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles creating enemy instances.
 */
public class Spawner : MonoBehaviour {

    public Enemy enemyPrefab;

    public Wave[] waves;

    public float spawnDelay;
    public Color tileFlashColor;
    public float tileFlashSpeed;

    public float timeBetweenCampingChecks;
    public float campThresholdDistance;

    Transform player;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    MapGenerator map;

    float nextCampCheckTime;
    Vector3 prevCampPosition;
    bool isCamping;

    bool isDisabled;

    void Start() {
        map = FindObjectOfType<MapGenerator>();
        LivingEntity playerEntity = FindObjectOfType<PlayerController>();
        player = playerEntity.transform;

        playerEntity.OnDeath += OnPlayerDeath;

        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        prevCampPosition = player.position;

        NextWave();
    }

    void Update() {
        if (isDisabled) {
            return;
        }

        if (Time.time > nextCampCheckTime) {
            nextCampCheckTime = Time.time + timeBetweenCampingChecks;
            isCamping = Vector3.Distance(player.position, prevCampPosition) < campThresholdDistance;
            prevCampPosition = player.position;
        }

        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.secsBetweenSpawns;

            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy() {
        // get the spawn tile
        Transform spawnTile = map.GetRandomOpenTile();
        if (isCamping) {
            spawnTile = map.GetTileFromPosition(player.position);
        }

        // flash the spawn tile
        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color initialColor = tileMat.color;
        float spawnTimer = 0;
        while (spawnTimer < spawnDelay) {
            tileMat.color = Color.Lerp(initialColor, tileFlashColor,
                    Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;

            yield return null;
        }
        tileMat.color = initialColor;

        // actually spawn the enemy
        Enemy spawnedEnemy = Instantiate(enemyPrefab,
                spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    void OnPlayerDeath() {
        isDisabled = true;
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
