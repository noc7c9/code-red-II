using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The map generator class.
 */
public class MapGenerator : MonoBehaviour {

    const string MAP_HOLDER_NAME = "Generated Map";

    public Map[] maps;
    public int mapIndex;

    public Transform tilePrefab;
    public Transform obstaclePrefab;

    public Transform navmesh;
    public Transform navmeshMaskPrefab;

    public Vector2 maxMapSize;
    public float tileSize;

    [Range(0, 1)]
    public float outlinePercent;

    Map currentMap;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    Queue<Coord> shuffledOpenTileCoords;
    Transform[,] tileMap;

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
        currentMap = maps[mapIndex];
        System.Random prng = new System.Random(currentMap.seed);

        GetComponent<BoxCollider>().size = new Vector3(
                currentMap.mapSize.x * tileSize,
                0.05f,
                currentMap.mapSize.y * tileSize);

        // create holder
        mapHolder = new GameObject(MAP_HOLDER_NAME).transform;
        mapHolder.parent = transform;

        // create coord collections
        allTileCoords = new List<Coord>();
        for (int x = 0; x < currentMap.mapSize.x; x++) {
            for (int y = 0; y < currentMap.mapSize.y; y++) {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(
                Utility.ShuffleArray(allTileCoords, prng.Next()));

        // create tile entities
        tileMap = new Transform[currentMap.mapSize.x, currentMap.mapSize.y];
        for (int x = 0; x < currentMap.mapSize.x; x++) {
            for (int y = 0; y < currentMap.mapSize.y; y++) {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab,
                        tilePosition, Quaternion.Euler(Vector3.right * 90))
                    as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newTile.parent = mapHolder;

                tileMap[x, y] = newTile;
            }
        }

        // create obstacles
        bool[,] obstacleMap = new bool[currentMap.mapSize.x, currentMap.mapSize.y];
        int targetObstacleCount = (int)(currentMap.tileCount * currentMap.obstaclePercent);
        int currentObstacleCount = 0;
        List<Coord> allOpenCoords = new List<Coord>(allTileCoords);

        for (int i = 0; i < targetObstacleCount; i++) {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (randomCoord != currentMap.mapCenter
                    && IsFullyAccessible(obstacleMap, currentObstacleCount)) {
                float obstacleHeight = Mathf.Lerp(
                        currentMap.minObstacleHeight, currentMap.maxObstacleHeight,
                        (float)prng.NextDouble());

                Vector3 obstaclePosition = CoordToPosition(randomCoord);
                Transform newObstacle = Instantiate(obstaclePrefab,
                        obstaclePosition + Vector3.up * obstacleHeight / 2,
                        Quaternion.identity) as Transform;

                newObstacle.parent = mapHolder;

                Vector3 scale = Vector3.one * (1 - outlinePercent) * tileSize;
                scale.y = obstacleHeight;
                newObstacle.localScale = scale;

                // set color
                Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
                Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
                obstacleMaterial.color = Color.Lerp(
                        currentMap.foregroundColor, currentMap.backgroundColor,
                        randomCoord.y / (float)currentMap.mapSize.y);
                obstacleRenderer.sharedMaterial = obstacleMaterial;

                allOpenCoords.Remove(randomCoord);
            } else {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }

        shuffledOpenTileCoords = new Queue<Coord>(
                Utility.ShuffleArray(allOpenCoords, prng.Next()));

        // setup navmesh with masks for map borders
        navmesh.localScale = new Vector3(maxMapSize.x, maxMapSize.y, 0) * tileSize;

        float horizontalOffset = (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize;
        Vector3 horizontalScale = new Vector3(
                (maxMapSize.x - currentMap.mapSize.x) / 2f,
                1,
                currentMap.mapSize.y) * tileSize;
        InstantiateNavmeshMask(mapHolder,
                Vector3.left * horizontalOffset, horizontalScale);
        InstantiateNavmeshMask(mapHolder,
                Vector3.right * horizontalOffset, horizontalScale);

        float verticalOffset = (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize;
        Vector3 verticalScale = new Vector3(
                maxMapSize.x,
                1,
                (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;
        InstantiateNavmeshMask(mapHolder,
                Vector3.forward * verticalOffset, verticalScale);
        InstantiateNavmeshMask(mapHolder,
                Vector3.back * verticalOffset, verticalScale);
    }

    void InstantiateNavmeshMask(Transform parent, Vector3 position, Vector3 scale) {
        Transform mask = Instantiate(navmeshMaskPrefab,
                position,
                Quaternion.identity) as Transform;
        mask.parent = parent;
        mask.localScale = scale;
    }

    bool IsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount) {
        int mapWidth = obstacleMap.GetLength(0);
        int mapHeight = obstacleMap.GetLength(1);
        bool[,] visited = new bool[mapWidth, mapHeight];
        Queue<Coord> queue = new Queue<Coord>();

        // start with the center
        queue.Enqueue(currentMap.mapCenter);
        visited[currentMap.mapCenter.x, currentMap.mapCenter.y] = true;
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

        return assessibleCount == (currentMap.tileCount - currentObstacleCount);
    }

    Vector3 CoordToPosition(Coord c) {
        return CoordToPosition(c.x, c.y);
    }

    Vector3 CoordToPosition(int x, int y) {
        return new Vector3(
                -currentMap.mapSize.x / 2f + 0.5f + x,
                0,
                -currentMap.mapSize.y / 2f + 0.5f + y) * tileSize;
    }

    public Transform GetTileFromPosition(Vector3 position) {
        int x = Mathf.RoundToInt(position.x / tileSize + (currentMap.mapSize.x - 1) / 2f);
        int y = Mathf.RoundToInt(position.z / tileSize + (currentMap.mapSize.y - 1) / 2f);
        x = Mathf.Clamp(x, 0, tileMap.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, tileMap.GetLength(1) - 1);
        return tileMap[x, y];
    }

    Coord GetRandomCoord() {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public Transform GetRandomOpenTile() {
        Coord randomCoord = shuffledOpenTileCoords.Dequeue();
        shuffledOpenTileCoords.Enqueue(randomCoord);
        return tileMap[randomCoord.x, randomCoord.y];
    }

    [System.Serializable]
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

        public override bool Equals(object obj) {
            if (obj == null || !(obj is Coord)) {
                return false;
            }
            return this == (Coord)obj;
        }

        public override int GetHashCode() {
            return 17 * (23 + x) * (23 + y);
        }

    }

    [System.Serializable]
    public class Map {

        public Coord mapSize;
        [Range(0, 1)]
        public float obstaclePercent;
        public int seed;
        public float minObstacleHeight;
        public float maxObstacleHeight;
        public Color foregroundColor;
        public Color backgroundColor;

        public Coord mapCenter {
            get {
                return new Coord(mapSize.x / 2, mapSize.y / 2);
            }
        }

        public int tileCount {
            get {
                return mapSize.x * mapSize.y;
            }
        }

    }

}
