namespace Noc7c9.TheDigitalFrontier {

    public enum TileType {
        Empty,
        Obstacle,
    }

    public interface ITile {
        TileType type { get; }
        Coord pos { get; }
    }

}
