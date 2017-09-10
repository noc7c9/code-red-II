using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Loads the prefabs for the given city block.
     */
    public class CityBlockLoader : MonoBehaviour {

        const string HOLDER_NAME = "City Block";

        public float pieceWidth;

        public List<RoadPrefabsItem> roadPrefabsList;
        Dictionary<RoadPiece, Transform> roadPrefabs;

        public List<PavementPrefabsItem> pavementPrefabsList;
        Dictionary<PavementPiece, Transform> pavementPrefabs;

        public List<BuildingPrefabsItem> buildingPrefabsList;
        Dictionary<BuildingPiece, Transform> buildingPrefabs;

        public List<MiscPrefabsItem> miscPrefabsList;
        Dictionary<MiscPiece, Transform> miscPrefabs;

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

            pavementPrefabs = new Dictionary<PavementPiece, Transform>();
            foreach (var item in pavementPrefabsList) {
                pavementPrefabs.Add(item.key, item.value);
            }

            buildingPrefabs = new Dictionary<BuildingPiece, Transform>();
            foreach (var item in buildingPrefabsList) {
                buildingPrefabs.Add(item.key, item.value);
            }

            miscPrefabs = new Dictionary<MiscPiece, Transform>();
            foreach (var item in miscPrefabsList) {
                miscPrefabs.Add(item.key, item.value);
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

            for (int x = 0; x < loadedCityBlock.size.x; x++) {
                for (int y = 0; y < loadedCityBlock.size.y; y++) {
                    // create road entities
                    {
                        var type = loadedCityBlock.GetRoadTile(x, y);
                        if (type != RoadPiece.NONE) {
                            var prefab = roadPrefabs[type];
                            Transform road = Instantiate(prefab) as Transform;
                            road.position += CoordToPosition(x, y, pieceWidth);
                            road.parent = loadedCityBlockHolder;
                        }
                    }

                    // create pavement entities
                    {
                        var type = loadedCityBlock.GetPavementTile(x, y);
                        if (type != PavementPiece.NONE) {
                            var prefab = pavementPrefabs[type];
                            Transform pavement = Instantiate(prefab) as Transform;
                            pavement.position += CoordToPosition(x, y, pieceWidth);
                            pavement.parent = loadedCityBlockHolder;
                        }
                    }

                    // create building entities
                    {
                        var type = loadedCityBlock.GetBuildingTile(x, y);
                        if (type != BuildingPiece.NONE) {
                            var prefab = buildingPrefabs[type];
                            Transform building = Instantiate(prefab) as Transform;
                            building.position += CoordToPosition(x, y, pieceWidth);
                            building.parent = loadedCityBlockHolder;
                        }
                    }

                    // create misc entities
                    {
                        var type = loadedCityBlock.GetMiscTile(x, y);
                        if (type != MiscPiece.NONE) {
                            var prefab = miscPrefabs[type];
                            Transform misc = Instantiate(prefab) as Transform;
                            misc.position += CoordToPosition(x, y, pieceWidth);
                            misc.parent = loadedCityBlockHolder;
                        }
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

    [System.Serializable]
    public struct PavementPrefabsItem {
        public PavementPiece key;
        public Transform value;
    }

    [System.Serializable]
    public struct BuildingPrefabsItem {
        public BuildingPiece key;
        public Transform value;
    }

    [System.Serializable]
    public struct MiscPrefabsItem {
        public MiscPiece key;
        public Transform value;
    }

}
