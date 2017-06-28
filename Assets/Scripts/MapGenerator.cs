using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The map generator class.
 */
public class MapGenerator : MonoBehaviour {

    const string MAP_HOLDER_NAME = "Generated Map";

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Vector2 mapSize;

    [Range(0, 1)]
    public float outlinePercent;
    public int seed;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;

    void Start() {
        GenerateMap();
    }

    public void GenerateMap() {
        // delete old map (if any)
        Transform mapHolder = transform.Find(MAP_HOLDER_NAME);
        if (mapHolder != null) {
            // immediate, since it must be runnable by an editor script
            DestroyImmediate(mapHolder.gameObject);
        }

        // create new map

        // create holder
        mapHolder = new GameObject(MAP_HOLDER_NAME).transform;
        mapHolder.parent = transform;

        // create coord collections
        allTileCoords = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(
                Utility.ShuffleArray(allTileCoords, seed));

        // create tile entities
        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab,
                        tilePosition, Quaternion.Euler(Vector3.right * 90))
                    as Transform;

                newTile.localScale = Vector3.one * (1 - outlinePercent);

                newTile.parent = mapHolder;
            }
        }

        // create obstacles
        int obstacleCount = 10;
        for (int i = 0; i < obstacleCount; i++) {
            Coord randomCoord = GetRandomCoord();
            Vector3 obstaclePosition = CoordToPosition(randomCoord);
            Transform newObstacle = Instantiate(obstaclePrefab,
                    obstaclePosition + Vector3.up * 0.5f,
                    Quaternion.identity) as Transform;
            newObstacle.parent = mapHolder;
        }
    }

    Vector3 CoordToPosition(Coord c) {
        return CoordToPosition(c.x, c.y);
    }

    Vector3 CoordToPosition(int x, int y) {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }

    Coord GetRandomCoord() {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public struct Coord {
        public int x;
        public int y;

        public Coord(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

}
