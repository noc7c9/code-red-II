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

        public CityBlock(CityBlockSettings settings) {
            this.settings = settings;

            size = new Coord(settings.width, settings.height);

            roads = new RoadPiece[size.x, size.y];
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

    }

    public enum RoadPiece {
        NONE,

        HORIZONTAL, HORIZONTAL_WITH_CROSSING,
        VERTICAL, VERTICAL_WITH_CROSSING,

        CROSS_CENTER,
        CROSS_SIDE_BOTTOM, CROSS_SIDE_LEFT,
        CROSS_SIDE_RIGHT,  CROSS_SIDE_TOP,
        CROSS_CORNER_LEFT_BOTTOM,  CROSS_CORNER_LEFT_TOP,
        CROSS_CORNER_RIGHT_BOTTOM, CROSS_CORNER_RIGHT_TOP,
    }

}
