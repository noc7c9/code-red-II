using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* The loads in the room.
     */
    public class RoomLoader : MonoBehaviour {

        const string HOLDER_NAME = "Generated Room";

        public Transform tilePrefab;
        public Transform obstaclePrefab;

        public Transform navmesh;
        public Transform navmeshMaskPrefab;
        public Vector2 navmeshSize;

        public Transform floor;

        public float tileSize;
        [Range(0, 1)]
        public float outlinePercent;

        Room loadedRoom;
        Transform loadedRoomHolder;

        Transform[,] tileMap;

        void Awake() {
            GameManager.Instance.GetSpawner().StartedNewWave
                += StartedNewWaveEventHandler;
        }

        void StartedNewWaveEventHandler(int waveNumber) {
            GenerateAndLoad(GameManager.Instance.GetRoomSettings(waveNumber - 1));
        }

        public void GenerateAndLoad(RoomSettings settings) {
            Load(RoomGenerator.Generate(settings));
        }

        public void Load(Room newRoom) {
            // delete old room (if any)
            if (loadedRoomHolder == null) {
                loadedRoomHolder = transform.Find(HOLDER_NAME);
            }
            if (loadedRoomHolder != null) {
                DestroyImmediate(loadedRoomHolder.gameObject);
            }

            // load new room
            loadedRoom = newRoom;

            // create holder
            loadedRoomHolder = new GameObject(HOLDER_NAME).transform;
            loadedRoomHolder.parent = transform;

            tileMap = new Transform[loadedRoom.size.x, loadedRoom.size.y];

            // create entities
            for (int x = 0; x < loadedRoom.size.x; x++) {
                for (int y = 0; y < loadedRoom.size.y; y++) {
                    // tile for every position in room
                    InstantiateTile(x, y);

                    ITile tile = loadedRoom.GetTile(x, y);
                    switch (tile.type) {
                        case TileType.Obstacle:
                            InstantiateObstacle((Obstacle) tile);
                        break;
                        default: break;
                    }
                }
            }

            SetupNavmeshMasks();

            floor.localScale = tileSize
                * new Vector3(loadedRoom.size.x, loadedRoom.size.y);
        }

        Vector3 CoordToPosition(Coord c) {
            return CoordToPosition(c.x, c.y);
        }

        Vector3 CoordToPosition(int x, int y) {
            return new Vector3(
                    -loadedRoom.size.x / 2f + 0.5f + x,
                    0,
                    -loadedRoom.size.y / 2f + 0.5f + y) * tileSize;
        }

        void SetupNavmeshMasks() {
            navmesh.localScale = tileSize
                * new Vector3(navmeshSize.x, navmeshSize.y, 0);

            float horizontalOffset = (loadedRoom.size.x + navmeshSize.x) / 4f * tileSize;
            Vector3 horizontalScale = new Vector3(
                    (navmeshSize.x - loadedRoom.size.x) / 2f,
                    1,
                    loadedRoom.size.y) * tileSize;
            InstantiateNavmeshMask(loadedRoomHolder,
                    Vector3.left * horizontalOffset, horizontalScale);
            InstantiateNavmeshMask(loadedRoomHolder,
                    Vector3.right * horizontalOffset, horizontalScale);

            float verticalOffset = (loadedRoom.size.y + navmeshSize.y) / 4f * tileSize;
            Vector3 verticalScale = new Vector3(
                    navmeshSize.x,
                    1,
                    (navmeshSize.y - loadedRoom.size.y) / 2f) * tileSize;
            InstantiateNavmeshMask(loadedRoomHolder,
                    Vector3.forward * verticalOffset, verticalScale);
            InstantiateNavmeshMask(loadedRoomHolder,
                    Vector3.back * verticalOffset, verticalScale);
        }

        void InstantiateNavmeshMask(Transform parent, Vector3 position, Vector3 scale) {
            Transform mask = Instantiate(navmeshMaskPrefab,
                    position,
                    Quaternion.identity) as Transform;
            mask.parent = parent;
            mask.localScale = scale;
        }

        void InstantiateTile(int x, int y) {
            Vector3 position = CoordToPosition(x, y);
            Transform newTile = Instantiate(tilePrefab,
                    position,
                    Quaternion.Euler(Vector3.right * 90)) as Transform;

            newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;

            newTile.parent = loadedRoomHolder;

            tileMap[x, y] = newTile;
        }

        void InstantiateObstacle(Obstacle obs) {
            Vector3 position = CoordToPosition(obs.pos);
            Transform newObstacle = Instantiate(obstaclePrefab,
                    position + Vector3.up * obs.height / 2,
                    Quaternion.identity) as Transform;

            // set height
            Vector3 scale = Vector3.one * (1 - outlinePercent) * tileSize;
            scale.y = obs.height;
            newObstacle.localScale = scale;

            // set color
            // note: done this way to avoid leaking materials in editor mode
            Renderer renderer = newObstacle.GetComponent<Renderer>();
            Material material = new Material(renderer.sharedMaterial);
            material.color = obs.color;
            renderer.sharedMaterial = material;

            newObstacle.parent = loadedRoomHolder;
        }

        public Transform GetTileFromPosition(Vector3 position) {
            int x = Mathf.RoundToInt(position.x / tileSize
                    + (loadedRoom.size.x - 1) / 2f);
            int y = Mathf.RoundToInt(position.z / tileSize
                    + (loadedRoom.size.y - 1) / 2f);
            x = Mathf.Clamp(x, 0, tileMap.GetLength(0) - 1);
            y = Mathf.Clamp(y, 0, tileMap.GetLength(1) - 1);
            return tileMap[x, y];
        }

        public Transform GetMapCenterTile() {
            return tileMap[loadedRoom.center.x, loadedRoom.center.y];
        }

        public Transform GetRandomOpenTile() {
            Coord c = loadedRoom.GetRandomOpenCoord();
            return tileMap[c.x, c.y];
        }

    }

}

