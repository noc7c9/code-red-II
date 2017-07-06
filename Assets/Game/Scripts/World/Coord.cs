namespace Noc7c9.TheDigitalFrontier {

    public struct Coord {

        public readonly int x;
        public readonly int y;

        readonly int hash;

        public Coord(int x, int y) {
            this.x = x;
            this.y = y;
            hash = 17 * (23 + x) * (23 + y);
        }

        public static bool operator ==(Coord c1, Coord c2) {
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator !=(Coord c1, Coord c2) {
            return !(c1 == c2);
        }

        public override bool Equals(object obj) {
            if (obj == null || !(obj is Coord)) {
                return false;
            }
            return this == (Coord)obj;
        }

        public override int GetHashCode() {
            return hash;
        }

    }

}
