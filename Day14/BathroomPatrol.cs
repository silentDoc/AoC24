using AoC24.Common;

namespace AoC24.Day14
{
    record BathroomRobot(Coord2D startPos, Coord2D velocity)
    {
        public Coord2D PosAfter(int steps, Coord2D dim)
            => (MathHelper.Modulo(startPos.x + velocity.x * steps, dim.x), MathHelper.Modulo(startPos.y + velocity.y * steps, dim.y));
    }

    internal class BathroomPatrol
    {
        List<BathroomRobot> robots = new();

        void ParseRobot(string line)
        { 
            var v = line.Replace("p=","").Replace(" v=", ",").Split(",").Select(int.Parse).ToList();
            robots.Add(new BathroomRobot((v[0], v[1]), (v[2], v[3])));
        }

        public void ParseInput(List<string> input)
            => input.ForEach(ParseRobot);

        int FindSafety()
        {
            Coord2D dims = (101,103);
            var middle = new Coord2D(50, 51);
            
            // Test data
            //Coord2D dims = (11, 7);
            //var middle = new Coord2D(5, 3);

            var robotsAfterSteps = robots.Select(x => x.PosAfter(100, dims)).ToList();
            

            var q1 = robotsAfterSteps.Count(r => r.x < middle.x && r.y < middle.y);
            var q2 = robotsAfterSteps.Count(r => r.x > middle.x && r.y < middle.y);
            var q3 = robotsAfterSteps.Count(r => r.x < middle.x && r.y > middle.y);
            var q4 = robotsAfterSteps.Count(r => r.x > middle.x && r.y > middle.y);

            return q1 * q2 * q3 * q4;
        }

        public int Solve(int part = 1)
            => FindSafety();
    }
}
