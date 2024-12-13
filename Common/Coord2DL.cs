namespace AoC24.Common
{
    public class Coord2DL : IEquatable<Coord2DL>
    {
        public enum Arrangement
        { 
            UpDownLeftRight = 0,
            UpRightDownLeft = 1
        }

        public long x = 0; 
        public long y = 0;

        public Coord2DL(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        public static Coord2DL operator +(Coord2DL coord_a, Coord2DL coord_b)
            => new Coord2DL(coord_a.x + coord_b.x, coord_a.y + coord_b.y);

        public static Coord2DL operator -(Coord2DL coord_a, Coord2DL coord_b)
            => new Coord2DL(coord_a.x - coord_b.x, coord_a.y - coord_b.y);

        public static Coord2DL operator *(Coord2DL coord, long scalar)
            => new Coord2DL(coord.x * scalar, coord.y * scalar);

        public static Coord2DL operator *(long scalar, Coord2DL coord)
            => new Coord2DL(coord.x * scalar, coord.y * scalar);
        public static Coord2DL operator /(Coord2DL coord, long scalar)
            => new Coord2DL(coord.x / scalar, coord.y / scalar);

        public static bool operator ==(Coord2DL coord_a, Coord2DL coord_b)
           => coord_a.Equals(coord_b);

        public static bool operator !=(Coord2DL coord_a, Coord2DL coord_b)
           => !coord_a.Equals(coord_b);

        public void Deconstruct(out long x, out long y)
        {
            x = this.x;
            y = this.y;
        }

        public bool Equals(Coord2DL? other)
            => other is null ? false : other.x == x && other.y == y;

        public override bool Equals(object? other) 
            => other is Coord2DL c && c.x.Equals(x)  && c.y.Equals(y);

        public static implicit operator (long, long)(Coord2DL c)       // Cast bw Coord and tuple
            => (c.x, c.y);

        public static implicit operator Coord2DL((long X, long Y) c) 
            => new Coord2DL(c.X, c.Y);

        public long Manhattan(Coord2DL other)
            => Math.Abs(x - other.x) + Math.Abs(y - other.y);

        public double DistanceTo(Coord2DL other)
            => (this - other).VectorModule;

        public double VectorModule
            => Math.Sqrt( x*x + y*y );

        public IEnumerable<Coord2DL> GetNeighbors(Arrangement arrange = Arrangement.UpRightDownLeft)
        {
            if (arrange == Arrangement.UpRightDownLeft)
            {
                // Up - Right - Down - Left
                yield return new Coord2DL(x, y - 1);
                yield return new Coord2DL(x + 1, y);
                yield return new Coord2DL(x, y + 1);
                yield return new Coord2DL(x - 1, y);
            }
            else
            {
                yield return new Coord2DL(x, y - 1);
                yield return new Coord2DL(x, y + 1);
                yield return new Coord2DL(x - 1, y);
                yield return new Coord2DL(x + 1, y);
            }
        }

        public IEnumerable<Coord2DL> GetNeighbors8()
        {
            yield return new Coord2DL(x + 1, y);
            yield return new Coord2DL(x + 1, y - 1);
            yield return new Coord2DL(x, y - 1);
            yield return new Coord2DL(x - 1, y - 1);
            yield return new Coord2DL(x - 1, y);
            yield return new Coord2DL(x - 1, y + 1);
            yield return new Coord2DL(x, y + 1);
            yield return new Coord2DL(x + 1, y + 1);
        }

        public override int GetHashCode()
        {
            unchecked // Wraps around max value
            {
                int hash = 17;
                hash = hash * 23 + (int)x;
                hash = hash * 23 + (int)y;
                return hash;
            }
        }

        public bool IsInside(long x0, long y0, long x1, long y1)
            => x >= Math.Min(x0, x1) &&
               x <= Math.Max(x0, x1) &&
               y >= Math.Min(y0, y1) &&
               y <= Math.Max(y0, y1);

        public bool IsInside(Coord2DL p0, Coord2DL p1)
            => IsInside(p0.x, p0.y, p1.x, p1.y);


        public double GetAngle()
        {
            var dirY = -1 * y;      // Up is down and viceversa
            var dirX =  x;

            double atan = Math.Atan2(dirY, dirX);
            double angle = (((360 - ((RadianToDegree(atan) + 360) % 360)) % 360) + 90) % 360;
            return angle;
        }

        private double RadianToDegree(double angle)
            => angle * (180.0 / Math.PI);

        public override string ToString()
        {
            return x.ToString() + "," + y.ToString();
        }


    }
}
