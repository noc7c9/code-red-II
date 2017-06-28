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
    public int tileCount {
        get {
            return (int)mapSize.x * (int)mapSize.y;
        }
    }

    [Range(0, 1)]
    public float outlinePercent;
    [Range(0, 1)]
    public float obstaclePercent;
    public int seed;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    Coord mapCenter;

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

        mapCenter = new Coord((int)mapSize.x/2, (int)mapSize.y/2);

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
        bool[,] obstacleMap = new bool[(int) mapSize.x, (int) mapSize.y];
        int targetObstacleCount = (int)(tileCount * obstaclePercent);
        int currentObstacleCount = 0;

        // there should be at least one free tile
        targetObstacleCount = System.Math.Min(targetObstacleCount, tileCount - 2);

        for (int i = 0; i < targetObstacleCount; i++) {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (randomCoord != mapCenter
                    && IsFullyAccessible(obstacleMap, currentObstacleCount)) {
                Vector3 obstaclePosition = CoordToPosition(randomCoord);
                Transform newObstacle = Instantiate(obstaclePrefab,
                        obstaclePosition + Vector3.up * 0.5f,
                        Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;
            } else {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }
    }

    bool IsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount) {
        int mapWidth = obstacleMap.GetLength(0);
        int mapHeight = obstacleMap.GetLength(1);
        bool[,] visited = new bool[mapWidth, mapHeight];
        Queue<Coord> queue = new Queue<Coord>();

        // start with the center
        queue.Enqueue(mapCenter);
        visited[mapCenter.x, mapCenter.y] = true;
        int assessibleCount = 1;

        while (queue.Count > 0) {
            Coord current = queue.Dequeue();

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    int neighbourX = current.x + x;
                    int neighbourY = current.y + y;

                    // ignore diagonals
                    if (x != 0 && y != 0) {
                        continue;
                    }

                    // ignore out of bound tiles
                    if (neighbourX < 0 || neighbourX >= mapWidth
                            || neighbourY < 0 || neighbourY >= mapHeight) {
                        continue;
                    }

                    // ignore visited
                    if (visited[neighbourX, neighbourY]) {
                        continue;
                    }

                    // ignore obstacles
                    if (obstacleMap[neighbourX, neighbourY]) {
                        continue;
                    }

                    // otherwise
                    visited[neighbourX, neighbourY] = true;
                    queue.Enqueue(new Coord(neighbourX, neighbourY));
                    assessibleCount++;
                }
            }
        }

        return assessibleCount == (tileCount - currentObstacleCount);
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

        public static bool operator ==(Coord c1, Coord c2) {
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator !=(Coord c1, Coord c2) {
            return !(c1 == c2);
        }

    }

}
