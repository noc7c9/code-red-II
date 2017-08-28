namespace Noc7c9.TheDigitalFrontier {

    /* Represents a single room.
     */
    public class Room {

        public readonly RoomSettings settings;
        public readonly Coord size;
        public readonly int tileCount;
        public readonly Coord center;

        ITile[,] tiles;
        bool[,] enemySpawnPoints;

        FisherYates.ShuffleList<Coord> openTiles;

        public Room(RoomSettings settings) {
            this.settings = settings;

            var width = settings.width;
            var height = settings.height;

            size = new Coord(width, height);
            tileCount = width * height;
            center = new Coord(width / 2, height / 2);

            tiles = new ITile[width, height];
            enemySpawnPoints = new bool[width, height];

            openTiles = new FisherYates.ShuffleList<Coord>(tileCount);
        }

        public void SetTile(Coord c, ITile tile) {
            SetTile(c.x, c.y, tile);
        }
        public void SetTile(int x, int y, ITile tile) {
            // change openTiles list if necessary
            if (tiles[x, y] == null || tiles[x, y].type != tile.type) {
                Coord c = new Coord(x, y);
                if (tile.type == TileType.Empty) {
                    openTiles.Add(c);
                } else {
                    openTiles.Remove(c);
                }
            }
            tiles[x, y] = tile;
        }

        public ITile GetTile(Coord c) {
            return GetTile(c.x, c.y);
        }
        public ITile GetTile(int x, int y) {
            return tiles[x, y];
        }

        public Coord GetRandomOpenCoord() {
            return openTiles.Next();
        }

        public void SetEnemySpawnPoint(Coord c) {
            SetEnemySpawnPoint(c.x, c.y);
        }
        public void SetEnemySpawnPoint(int x, int y) {
            enemySpawnPoints[x, y] = true;
        }

        public bool IsEnemySpawnPoint(Coord c) {
            return IsEnemySpawnPoint(c.x, c.y);
        }
        public bool IsEnemySpawnPoint(int x, int y) {
            return enemySpawnPoints[x, y];
        }


    }

}
