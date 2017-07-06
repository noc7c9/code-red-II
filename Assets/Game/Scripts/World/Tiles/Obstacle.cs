using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public struct Obstacle : ITile {

        public TileType type {
            get {
                return TileType.Obstacle;
            }
        }

        public Coord pos { get; }

        public readonly float height;
        public readonly Color color;

        public Obstacle(int x, int y, float height, Color color)
            : this (new Coord(x, y), height, color) {}
        public Obstacle(Coord pos, float height, Color color) {
            this.pos = pos;
            this.height = height;
            this.color = color;
        }

    }

}
