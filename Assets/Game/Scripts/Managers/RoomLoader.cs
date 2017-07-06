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

        public RoomSettings roomSettings;

        Room currentRoom;
        Transform currentRoomTransform;

        public void Generate() {
            // delete old room (if any)
            if (currentRoomTransform == null) {
                currentRoomTransform = transform.Find(HOLDER_NAME);
            }
            if (currentRoomTransform != null) {
                DestroyImmediate(currentRoomTransform.gameObject);
            }

            // create new room
            currentRoom = RoomGenerator.Generate(roomSettings);

            // create holder
            currentRoomTransform = new GameObject(HOLDER_NAME).transform;
            currentRoomTransform.parent = transform;

            // create entities
            for (int x = 0; x < currentRoom.size.x; x++) {
                for (int y = 0; y < currentRoom.size.y; y++) {
                    // tile for every position in room
                    InstantiateTile(x, y);

                    ITile tile = currentRoom.GetTile(x, y);
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
                * new Vector3(currentRoom.size.x, currentRoom.size.y);
        }

        Vector3 CoordToPosition(Coord c) {
            return CoordToPosition(c.x, c.y);
        }

        Vector3 CoordToPosition(int x, int y) {
            return new Vector3(
                    -currentRoom.size.x / 2f + 0.5f + x,
                    0,
                    -currentRoom.size.y / 2f + 0.5f + y) * tileSize;
        }

        void SetupNavmeshMasks() {
            navmesh.localScale = new Vector3(navmeshSize.x, navmeshSize.y, 0) * tileSize;

            float horizontalOffset = (currentRoom.size.x + navmeshSize.x) / 4f * tileSize;
            Vector3 horizontalScale = new Vector3(
                    (navmeshSize.x - currentRoom.size.x) / 2f,
                    1,
                    currentRoom.size.y) * tileSize;
            InstantiateNavmeshMask(currentRoomTransform,
                    Vector3.left * horizontalOffset, horizontalScale);
            InstantiateNavmeshMask(currentRoomTransform,
                    Vector3.right * horizontalOffset, horizontalScale);

            float verticalOffset = (currentRoom.size.y + navmeshSize.y) / 4f * tileSize;
            Vector3 verticalScale = new Vector3(
                    navmeshSize.x,
                    1,
                    (navmeshSize.y - currentRoom.size.y) / 2f) * tileSize;
            InstantiateNavmeshMask(currentRoomTransform,
                    Vector3.forward * verticalOffset, verticalScale);
            InstantiateNavmeshMask(currentRoomTransform,
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

            newTile.parent = currentRoomTransform;
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

            newObstacle.parent = currentRoomTransform;
        }

    }

}

