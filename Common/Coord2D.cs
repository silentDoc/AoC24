using System.Collections.Generic;

namespace AoC24.Common
{
    public class Coord2D : IEquatable<Coord2D>
    {
        public enum Arrangement
        { 
            UpDownLeftRight = 0,
            UpRightDownLeft = 1
        }

        public int x = 0; 
        public int y = 0;

        public Coord2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Coord2D operator +(Coord2D coord_a, Coord2D coord_b)
            => new Coord2D(coord_a.x + coord_b.x, coord_a.y + coord_b.y);

        public static Coord2D operator -(Coord2D coord_a, Coord2D coord_b)
            => new Coord2D(coord_a.x - coord_b.x, coord_a.y - coord_b.y);

        public static Coord2D operator *(Coord2D coord, int scalar)
            => new Coord2D(coord.x * scalar, coord.y * scalar);

        public static Coord2D operator *(int scalar, Coord2D coord)
            => new Coord2D(coord.x * scalar, coord.y * scalar);
        public static Coord2D operator /(Coord2D coord, int scalar)
            => new Coord2D(coord.x / scalar, coord.y / scalar);

        public static bool operator ==(Coord2D coord_a, Coord2D coord_b)
           => coord_a.Equals(coord_b);

        public static bool operator !=(Coord2D coord_a, Coord2D coord_b)
           => !coord_a.Equals(coord_b);

        public void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }

        public bool Equals(Coord2D? other)
            => other is null ? false : other.x == x && other.y == y;

        public override bool Equals(object? other) 
            => other is Coord2D c && c.x.Equals(x)  && c.y.Equals(y);

        public static implicit operator (int, int)(Coord2D c)       // Cast bw Coord and tuple
            => (c.x, c.y);

        public static implicit operator Coord2D((int X, int Y) c) 
            => new Coord2D(c.X, c.Y);

        public int Manhattan(Coord2D other)
            => Math.Abs(x - other.x) + Math.Abs(y - other.y);

        public double DistanceTo(Coord2D other)
            => (this - other).VectorModule;

        public double VectorModule
            => Math.Sqrt( x*x + y*y );

        public IEnumerable<Coord2D> GetNeighbors(int dist = 1, Arrangement arrange = Arrangement.UpRightDownLeft)
        {
            if (arrange == Arrangement.UpRightDownLeft)
            {
                // Up - Right - Down - Left
                yield return new Coord2D(x, y - dist);
                yield return new Coord2D(x + dist, y);
                yield return new Coord2D(x, y + dist);
                yield return new Coord2D(x - dist, y);
            }
            else
            {
                yield return new Coord2D(x, y - dist);
                yield return new Coord2D(x, y + dist);
                yield return new Coord2D(x - dist, y);
                yield return new Coord2D(x + dist, y);
            }
        }

        public IEnumerable<Coord2D> GetNeighbors8(int dist = 1)
        {
            yield return new Coord2D(x + dist, y);
            yield return new Coord2D(x + dist, y - 1);
            yield return new Coord2D(x, y - dist);
            yield return new Coord2D(x - dist, y - dist);
            yield return new Coord2D(x - dist, y);
            yield return new Coord2D(x - dist, y + dist);
            yield return new Coord2D(x, y + dist);
            yield return new Coord2D(x + dist, y + dist);
        }

        public override int GetHashCode()
        {
            unchecked // Wraps around max value
            {
                int hash = 17;
                hash = hash * 23 + x;
                hash = hash * 23 + y;
                return hash;
            }
        }

        public bool IsInside(int x0, int y0, int x1, int y1)
            => x >= Math.Min(x0, x1) &&
               x <= Math.Max(x0, x1) &&
               y >= Math.Min(y0, y1) &&
               y <= Math.Max(y0, y1);

        public bool IsInside(Coord2D p0, Coord2D p1)
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
