using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Handles creating enemy instances.
     */
    public class Spawner : MonoBehaviour {

        public Enemy enemyPrefab;

        public float playerSpawnHeight;

        public float spawnDelay;
        public Color tileFlashColor;
        public float tileFlashSpeed;

        public float timeBetweenCampingChecks;
        public float campThresholdDistance;

        Transform player;

        public int startingWaveNumber;

        Wave currentWave;
        int currentWaveNumber;

        int enemiesRemainingToSpawn;
        int enemiesRemainingAlive;
        float nextSpawnTime;

        RoomLoader roomLoader;

        float nextCampCheckTime;
        Vector3 prevCampPosition;
        bool isCamping;

        bool isDisabled;

        public event System.Action<int> StartedNewWave;
        protected virtual void OnStartedNewWave(int number) {
            var evt = StartedNewWave;
            if (evt != null) {
                evt(number);
            }
        }

        void Start() {
            roomLoader = GameManager.Instance.GetRoomLoader();

            LivingEntity playerEntity = GameManager.Instance.GetPlayerController();
            player = playerEntity.transform;

            playerEntity.Dying += PlayerDyingEventHandler;

            nextCampCheckTime = timeBetweenCampingChecks + Time.time;
            prevCampPosition = player.position;

            currentWaveNumber = startingWaveNumber - 1;
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

            if ((currentWave.infinite || enemiesRemainingToSpawn > 0)
                    && Time.time > nextSpawnTime) {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.secsBetweenSpawns;

                StartCoroutine(SpawnEnemy());
            }
        }

        IEnumerator SpawnEnemy() {
            // get the spawn tile
            Transform spawnTile = roomLoader.GetRandomOpenTile();
            if (isCamping) {
                spawnTile = roomLoader.GetTileFromPosition(player.position);
            }

            // flash the spawn tile
            Tile tile = spawnTile.GetComponent<Tile>();
            yield return StartCoroutine(
                    tile.Flash(tileFlashColor, spawnDelay, tileFlashSpeed));

            // actually spawn the enemy
            Enemy spawnedEnemy = Instantiate(enemyPrefab,
                    spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
            spawnedEnemy.Dying += EnemyDyingEventHandler;
            spawnedEnemy.SetCharacteristics(
                    currentWave.enemyMoveSpeed, currentWave.enemyDamage,
                    currentWave.enemyHealth, currentWave.enemyColor);
        }

        void PlayerDyingEventHandler() {
            isDisabled = true;
        }

        void EnemyDyingEventHandler() {
            enemiesRemainingAlive--;

            if (enemiesRemainingAlive == 0) {
                NextWave();
            }
        }

        void ResetPlayerPosition() {
            Transform center = roomLoader.GetMapCenterTile();
            if (center != null) {
                player.position = center.position + Vector3.up * playerSpawnHeight;
            }
        }

        void NextWave() {
            if (currentWaveNumber > 0) {
                AudioManager.Instance.PlaySound2D("Level Complete");
            }

            currentWaveNumber++;
            if (currentWaveNumber - 1 < GameManager.Instance.GetLevelsCount()) {
                currentWave = GameManager.Instance.GetWave(currentWaveNumber - 1);

                enemiesRemainingToSpawn = currentWave.enemyCount;
                enemiesRemainingAlive = enemiesRemainingToSpawn;

                ResetPlayerPosition();

                OnStartedNewWave(currentWaveNumber);
            }
        }

        [System.Serializable]
        public class Wave {
            public bool infinite;
            public int enemyCount;
            public float secsBetweenSpawns;

            public float enemyMoveSpeed;
            public float enemyDamage;
            public float enemyHealth;
            public Color enemyColor;
        }

    }

}
