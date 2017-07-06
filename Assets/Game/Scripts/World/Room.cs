namespace Noc7c9.TheDigitalFrontier {

    public class Room {

        public readonly Coord size;
        public readonly int tileCount;
        public readonly Coord center;

        ITile[,] tiles;

        public Room(int width, int height) {
            size = new Coord(width, height);
            tileCount = width * height;
            center = new Coord(width / 2, height / 2);

            tiles = new ITile[width, height];
        }

        public void SetTile(Coord c, ITile tile) {
            SetTile(c.x, c.y, tile);
        }
        public void SetTile(int x, int y, ITile tile) {
            tiles[x, y] = tile;
        }

        public ITile GetTile(Coord c) {
            return GetTile(c.x, c.y);
        }
        public ITile GetTile(int x, int y) {
            return tiles[x, y];
        }

    }

}
