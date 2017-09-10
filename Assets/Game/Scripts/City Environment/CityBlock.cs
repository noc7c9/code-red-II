using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Describes a single city block.
     */
    public class CityBlock {

        public readonly CityBlockSettings settings;
        public readonly Coord size;

        RoadPiece[,] roads;
        PavementPiece[,] pavements;
        BuildingPiece[,] buildings;
        MiscPiece[,] misc;

        public CityBlock(CityBlockSettings settings) {
            this.settings = settings;

            size = new Coord(settings.width, settings.height);

            roads = new RoadPiece[size.x, size.y];
            pavements = new PavementPiece[size.x, size.y];
            buildings = new BuildingPiece[size.x, size.y];
            misc = new MiscPiece[size.x, size.y];
        }

        public void SetRoadTile(Coord c, RoadPiece value) {
            SetRoadTile(c.x, c.y, value);
        }
        public void SetRoadTile(int x, int y, RoadPiece value) {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y) {
                return;
            }
            roads[x, y] = value;
        }

        public RoadPiece GetRoadTile(Coord c) {
            return GetRoadTile(c.x, c.y);
        }
        public RoadPiece GetRoadTile(int x, int y) {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y) {
                return RoadPiece.NONE;
            }
            return roads[x, y];
        }

        public void SetPavementTile(Coord c, PavementPiece value) {
            SetPavementTile(c.x, c.y, value);
        }
        public void SetPavementTile(int x, int y, PavementPiece value) {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y) {
                return;
            }
            pavements[x, y] = value;
        }

        public PavementPiece GetPavementTile(Coord c) {
            return GetPavementTile(c.x, c.y);
        }
        public PavementPiece GetPavementTile(int x, int y) {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y) {
                return PavementPiece.NONE;
            }
            return pavements[x, y];
        }

        public void SetBuildingTile(Coord c, BuildingPiece value) {
            SetBuildingTile(c.x, c.y, value);
        }
        public void SetBuildingTile(int x, int y, BuildingPiece value) {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y) {
                return;
            }
            buildings[x, y] = value;
        }

        public BuildingPiece GetBuildingTile(Coord c) {
            return GetBuildingTile(c.x, c.y);
        }
        public BuildingPiece GetBuildingTile(int x, int y) {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y) {
                return BuildingPiece.NONE;
            }
            return buildings[x, y];
        }

        public void SetMiscTile(Coord c, MiscPiece value) {
            SetMiscTile(c.x, c.y, value);
        }
        public void SetMiscTile(int x, int y, MiscPiece value) {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y) {
                return;
            }
            misc[x, y] = value;
        }

        public MiscPiece GetMiscTile(Coord c) {
            return GetMiscTile(c.x, c.y);
        }
        public MiscPiece GetMiscTile(int x, int y) {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y) {
                return MiscPiece.NONE;
            }
            return misc[x, y];
        }

    }

    public enum RoadPiece {
        NONE,

        HORIZONTAL, HORIZONTAL_WITH_CROSSING,
        VERTICAL, VERTICAL_WITH_CROSSING,

        CROSS_CENTER,
        CROSS_S, CROSS_W, CROSS_E, CROSS_N,
        CROSS_SW, CROSS_NW, CROSS_SE, CROSS_NE,
    }

    public enum PavementPiece {
        NONE,
        CORNER_NE, CORNER_SE, CORNER_SW, CORNER_NW,
        SIDE_N, SIDE_E, SIDE_S, SIDE_W,
    }

    public enum BuildingPiece {
        NONE,
        CORNER_NE, CORNER_SE, CORNER_SW, CORNER_NW,
        SIDE_N, SIDE_E, SIDE_S, SIDE_W,
        CORNER_INNER_NE, CORNER_INNER_SE, CORNER_INNER_SW, CORNER_INNER_NW,
    }

    public enum MiscPiece {
        NONE,

        STREET_LAMP_CORNER_NE, STREET_LAMP_CORNER_SE,
        STREET_LAMP_CORNER_NW, STREET_LAMP_CORNER_SW,
    }

}
