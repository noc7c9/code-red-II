using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The map generator class.
 */
public class MapGenerator : MonoBehaviour {

    const string MAP_HOLDER_NAME = "Generated Map";

    public Transform tilePrefab;
    public Vector2 mapSize;

    [Range(0, 1)]
    public float outlinePercent;

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
        mapHolder = new GameObject(MAP_HOLDER_NAME).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {
                Vector3 tilePosition = new Vector3(
                        -mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
                Transform newTile = Instantiate(tilePrefab,
                        tilePosition, Quaternion.Euler(Vector3.right * 90))
                    as Transform;

                newTile.localScale = Vector3.one * (1 - outlinePercent);

                newTile.parent = mapHolder;
            }
        }
    }

}
