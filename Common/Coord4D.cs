namespace AoC24.Common
{
    public class Coord4D : IEquatable<Coord4D>
    {
        public enum Arrangement
        { 
            UpDownLeftRight = 0,
            UpRightDownLeft = 1
        }

        public int x = 0; 
        public int y = 0;
        public int z = 0;
        public int t = 0;


        public Coord4D(int x, int y, int z, int t)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.t = t;

        }

        public static Coord4D operator +(Coord4D coord_a, Coord4D coord_b)
            => new Coord4D(coord_a.x + coord_b.x, coord_a.y + coord_b.y, coord_a.z + coord_b.z, coord_a.t + coord_b.t);

        public static Coord4D operator -(Coord4D coord_a, Coord4D coord_b)
            => new Coord4D(coord_a.x - coord_b.x, coord_a.y - coord_b.y, coord_a.z - coord_b.z, coord_a.t - coord_b.t);

        public static Coord4D operator *(Coord4D coord, int scalar)
            => new Coord4D(coord.x * scalar, coord.y * scalar, coord.z * scalar, coord.t * scalar);

        public static Coord4D operator *(int scalar, Coord4D coord)
            => new Coord4D(coord.x * scalar, coord.y * scalar, coord.z * scalar, coord.t * scalar);
        public static Coord4D operator /(Coord4D coord, int scalar)
            => new Coord4D(coord.x / scalar, coord.y / scalar, coord.z/scalar, coord.t/scalar);

        public static bool operator ==(Coord4D coord_a, Coord4D coord_b)
           => coord_a.Equals(coord_b);

        public static bool operator !=(Coord4D coord_a, Coord4D coord_b)
           => !coord_a.Equals(coord_b);

        public void Deconstruct(out int x, out int y, out int z, out int t)
        {
            x = this.x;
            y = this.y;
            z = this.z;
            t = this.t;
        }

        public IEnumerable<Coord2D> GetNeighborsXY()
        {
            // Up - Right - Down - Left
            yield return new Coord2D(x, y - 1);
            yield return new Coord2D(x + 1, y);
            yield return new Coord2D(x, y + 1);
            yield return new Coord2D(x - 1, y);
        }

        public IEnumerable<Coord4D> GetNeighbors8()
        {
            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                    for (int k = z - 1; k <= z + 1; k++)
                        for (int m = t - 1; m <= t + 1; m++)
                        {
                            if (i == x && j == y && k == z && m == t)
                                continue;
                            yield return (i, j, k, m);
                        }
        }

        public bool Equals(Coord4D? other)
            => other is null ? false : other.x == x && other.y == y && other.z == z && other.t == t;

        public override bool Equals(object? other) 
            => other is Coord4D c && c.x.Equals(x)  && c.y.Equals(y) && c.z.Equals(z) && c.t.Equals(t);

        public static implicit operator (int, int, int, int)(Coord4D c)       // Cast bw Coord and tuple
            => (c.x, c.y, c.z, c.t);

        public static implicit operator Coord4D((int X, int Y, int Z, int T) c) 
            => new Coord4D(c.X, c.Y, c.Z, c.T);

        public int Manhattan(Coord4D other)
            => Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z) + Math.Abs(t - other.t);

        public double VectorModule
            => Math.Sqrt(x * x + y * y + z * z + t * t);

        public override int GetHashCode()
        {
            unchecked // Wraps around max value
            {
                int hash = 17;
                hash = hash * 23 + x;
                hash = hash * 23 + y;
                hash = hash * 23 + z;
                hash = hash * 23 + t;
                return hash;
            }
        }

        
    }
}
