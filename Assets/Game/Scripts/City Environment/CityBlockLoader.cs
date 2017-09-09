using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Loads the prefabs for the given city block.
     */
    public class CityBlockLoader : MonoBehaviour {

        const string HOLDER_NAME = "City Block";

        public float roadPieceWidth;
        public List<RoadPrefabsItem> roadPrefabsList;
        Dictionary<RoadPiece, Transform> roadPrefabs;

        CityBlock loadedCityBlock;
        Transform loadedCityBlockHolder;

        public void Awake() {
            SetupDictionaries();
        }

        bool dictionariesSetup;
        void SetupDictionaries() {
            #if !UNITY_EDITOR
                if (dictionariesSetup) {
                    return;
                }
            #endif

            roadPrefabs = new Dictionary<RoadPiece, Transform>();
            foreach (var item in roadPrefabsList) {
                roadPrefabs.Add(item.key, item.value);
            }

            dictionariesSetup = true;
        }

        public void Load(CityBlock cityBlock) {
            SetupDictionaries();

            // delete old city block (if any)
            if (loadedCityBlockHolder == null) {
                loadedCityBlockHolder = transform.Find(HOLDER_NAME);
            }
            if (loadedCityBlockHolder != null) {
                DestroyImmediate(loadedCityBlockHolder.gameObject);
            }

            // load new city block
            loadedCityBlock = cityBlock;

            // create holder
            loadedCityBlockHolder = new GameObject(HOLDER_NAME).transform;
            loadedCityBlockHolder.parent = transform;

            // create road entities
            for (int x = 0; x < loadedCityBlock.size.x; x++) {
                for (int y = 0; y < loadedCityBlock.size.y; y++) {
                    var type = loadedCityBlock.GetRoadTile(x, y);
                    if (type != RoadPiece.NONE) {
                        var prefab = roadPrefabs[type];
                        Transform road = Instantiate(prefab) as Transform;
                        road.position = CoordToPosition(x, y, roadPieceWidth);
                        road.parent = loadedCityBlockHolder;
                    }
                }
            }
        }

        Vector3 CoordToPosition(Coord c, float pieceWidth) {
            return CoordToPosition(c.x, c.y, pieceWidth);
        }

        Vector3 CoordToPosition(int x, int y, float pieceWidth) {
            return new Vector3(x, 0, y) * pieceWidth;
        }

    }

    [System.Serializable]
    public struct RoadPrefabsItem {
        public RoadPiece key;
        public Transform value;
    }

}
