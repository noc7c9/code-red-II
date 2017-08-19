namespace Noc7c9.TheDigitalFrontier {

    public struct EmptyTile : ITile {

        public TileType type {
            get {
                return TileType.Empty;
            }
        }

        public Coord pos { get; }

        public EmptyTile(int x, int y) : this(new Coord(x, y)) {}
        public EmptyTile(Coord pos) {
            this.pos = pos;
        }

    }

}
