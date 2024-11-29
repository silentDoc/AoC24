namespace AoC24.Common
{
    public class Coord3D : IEquatable<Coord3D>
    {
        public enum Arrangement
        { 
            UpDownLeftRight = 0,
            UpRightDownLeft = 1
        }

        public int x = 0; 
        public int y = 0;
        public int z = 0;


        public Coord3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Coord3D operator +(Coord3D coord_a, Coord3D coord_b)
            => new Coord3D(coord_a.x + coord_b.x, coord_a.y + coord_b.y, coord_a.z + coord_b.z);

        public static Coord3D operator -(Coord3D coord_a, Coord3D coord_b)
            => new Coord3D(coord_a.x - coord_b.x, coord_a.y - coord_b.y, coord_a.z - coord_b.z);

        public static Coord3D operator *(Coord3D coord, int scalar)
            => new Coord3D(coord.x * scalar, coord.y * scalar, coord.z * scalar);

        public static Coord3D operator *(int scalar, Coord3D coord)
            => new Coord3D(coord.x * scalar, coord.y * scalar, coord.z * scalar);
        public static Coord3D operator /(Coord3D coord, int scalar)
            => new Coord3D(coord.x / scalar, coord.y / scalar, coord.z/scalar);

        public static bool operator ==(Coord3D coord_a, Coord3D coord_b)
           => coord_a.Equals(coord_b);

        public static bool operator !=(Coord3D coord_a, Coord3D coord_b)
           => !coord_a.Equals(coord_b);

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }

        public IEnumerable<Coord2D> GetNeighborsXY()
        {
            // Up - Right - Down - Left
            yield return new Coord2D(x, y - 1);
            yield return new Coord2D(x + 1, y);
            yield return new Coord2D(x, y + 1);
            yield return new Coord2D(x - 1, y);
        }

        public IEnumerable<Coord3D> GetNeighbors()
        {
            yield return (x, y, z - 1);
            yield return (x, y, z + 1);
            yield return (x, y - 1, z);
            yield return (x, y + 1, z);
            yield return (x - 1, y, z);
            yield return (x + 1, y, z);
        }

        public IEnumerable<Coord3D> GetNeighbors8()
        {
            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                    for (int k = z - 1; k <= z + 1; k++)
                        {
                            if (i == x && j == y && k == z)
                                continue;
                            yield return (i, j, k);
                        }
        }


        public bool Equals(Coord3D? other)
            => other is null ? false : other.x == x && other.y == y && other.z == z;

        public override bool Equals(object? other) 
            => other is Coord3D c && c.x.Equals(x)  && c.y.Equals(y) && c.z.Equals(z);

        public static implicit operator (int, int, int)(Coord3D c)       // Cast bw Coord and tuple
            => (c.x, c.y, c.z);

        public static implicit operator Coord3D((int X, int Y, int Z) c) 
            => new Coord3D(c.X, c.Y, c.Z);

        public int Manhattan(Coord3D other)
            => Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z);

        public double VectorModule
            => Math.Sqrt(x * x + y * y + z * z);

        public override int GetHashCode()
        {
            unchecked // Wraps around max value
            {
                int hash = 17;
                hash = hash * 23 + x;
                hash = hash * 23 + y;
                hash = hash * 23 + z;
                return hash;
            }
        }
        public override string ToString()
        {
            return x.ToString() + "," + y.ToString() + "," + z.ToString();
        }


    }
}
