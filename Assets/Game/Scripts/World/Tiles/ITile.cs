namespace Noc7c9.TheDigitalFrontier {

    public enum TileType {
        Empty,
        Obstacle,
        Warp,
    }

    public interface ITile {
        TileType type { get; }
        Coord pos { get; }
    }

}
