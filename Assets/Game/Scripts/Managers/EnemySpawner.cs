using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class EnemySpawner : MonoBehaviour {

        public Transform enemyPrefab;

        public float playerSafeRadius;

        [Range(0, 1)]
        public float chanceOfEnemySpawnOnRoadPiece;

        [Range(0, 100)]
        public int minEnemySpawnGroupSize;

        [Range(1, 100)]
        public int maxEnemySpawnGroupSize;

        public void PopulateStage(CityBlock cityBlock, float pieceWidth) {
            var player = GameManager.Instance.GetPlayerController().transform;

            for (int x = 0; x < cityBlock.size.x; x++) {
                for (int y = 0; y < cityBlock.size.y; y++) {
                    if (cityBlock.GetRoadTile(x, y) == RoadPiece.NONE) {
                        continue;
                    }

                    if (Random.value <= chanceOfEnemySpawnOnRoadPiece) {
                        int numEnemies = Random.Range(
                                minEnemySpawnGroupSize, maxEnemySpawnGroupSize);
                        for (int i = 0; i < numEnemies; i++) {
                            Vector3 position = CoordToPosition(x, y, pieceWidth);
                            position.x +=
                                Random.Range(-pieceWidth, pieceWidth) / 2;
                            position.z +=
                                Random.Range(-pieceWidth, pieceWidth) / 2;

                            var distToPlayer = (player.position - position).magnitude;
                            if (distToPlayer <= playerSafeRadius) {
                                continue;
                            }

                            CreateEnemy(position);
                        }
                    }
                }
            }
        }

        void CreateEnemy(Vector3 position) {
            Instantiate(enemyPrefab, position, Quaternion.identity);
            // Debug.Log("Spawn Enemy", position);
        }

        Vector3 CoordToPosition(int x, int y, float pieceWidth) {
            return new Vector3(x, 0, y) * pieceWidth;
        }

    }

}
