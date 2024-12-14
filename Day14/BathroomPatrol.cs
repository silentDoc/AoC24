using AoC24.Common;
using System.Text;

namespace AoC24.Day14
{
    record BathroomRobot(Coord2D startPos, Coord2D velocity)
    {
        public Coord2D PosAfter(int steps, Coord2D dim)
            => (MathHelper.Modulo(startPos.x + velocity.x * steps, dim.x), MathHelper.Modulo(startPos.y + velocity.y * steps, dim.y));
    }

    internal class BathroomPatrol
    {
        HashSet<BathroomRobot> robots = new();

        void ParseRobot(string line)
        { 
            var v = line.Replace("p=","").Replace(" v=", ",").Split(",").Select(int.Parse).ToList();
            robots.Add(new BathroomRobot((v[0], v[1]), (v[2], v[3])));
        }

        public void ParseInput(List<string> input)
            => input.ForEach(ParseRobot);

        void Display(HashSet<Coord2D> robots, int step)
        {
            
            int maxX = robots.Max(r => r.x);
            int maxY = robots.Max(r => r.y);

            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Step : " + step.ToString());
            Console.WriteLine();

            for (int y = 0; y <= maxY; y++)
            {
                StringBuilder sb = new();
                for (int x = 0; x <= maxX; x++)
                {
                    Coord2D pos = (x, y);
                    sb.Append(robots.Contains(pos) ? '#' : '.');
                }
                Console.WriteLine(sb.ToString());
            }
        }

        int FindTree()
        {
            Coord2D dims = (101, 103);
            int steps = 0;
            int numRobots = robots.Count();

            while (true)
            {
                var robotsAfterSteps = robots.Select(x => x.PosAfter(steps, dims)).ToHashSet();
                var together = robotsAfterSteps.Count(x => x.GetNeighbors().Any(n => robotsAfterSteps.Contains(n)));

                // We want more than half of the robots to be close to each other
                if (together > numRobots / 2)
                {
                    Display(robotsAfterSteps, steps);
                    Console.ReadKey();
                    break;
                }
                steps++;
            }
            return steps;
        }

        int FindSafety()
        {
            Coord2D dims = (101,103);
            var middle = new Coord2D(50, 51);

            var robotsAfterSteps = robots.Select(x => x.PosAfter(100, dims)).ToList();
            var q1 = robotsAfterSteps.Count(r => r.x < middle.x && r.y < middle.y);
            var q2 = robotsAfterSteps.Count(r => r.x > middle.x && r.y < middle.y);
            var q3 = robotsAfterSteps.Count(r => r.x < middle.x && r.y > middle.y);
            var q4 = robotsAfterSteps.Count(r => r.x > middle.x && r.y > middle.y);

            return q1 * q2 * q3 * q4;
        }

        public int Solve(int part = 1)
            => part == 1 ? FindSafety() : FindTree();
    }
}
