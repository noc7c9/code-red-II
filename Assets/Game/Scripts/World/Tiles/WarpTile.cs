using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public struct WarpTile : ITile {

        public TileType type {
            get {
                return TileType.Warp;
            }
        }

        public Coord pos { get; }

        public int target { get; }

        public WarpTile(int x, int y, int target)
            : this (new Coord(x, y), target) {}
        public WarpTile(Coord pos, int target) {
            this.pos = pos;
            this.target = target;
        }

    }

}
