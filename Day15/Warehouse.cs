using AoC24.Common;
using System.Text;

namespace AoC24.Day15
{
    internal class Warehouse
    {
        string moveSet = "";
        Dictionary<Coord2D, char> map = new();
        int maxX = 0;
        int maxY = 0;

        public void ParseInput(List<string> input)
        {
            var sections = ParseUtils.SplitBy(input, "");
            moveSet = string.Concat(sections[1]);

            sections[0].Index().ToList().ForEach(element => ParseLine(element.Index, element.Item));
            maxX = map.Keys.Max(k => k.x);
            maxY = map.Keys.Max(k => k.y);
        }

        void ParseLine(int row, string line)
            => line.Index().ToList().ForEach(c => map[(c.Index, row)] = c.Item);

        Coord2D Step(Coord2D robotPos, char dir)
        {
            var scan = dir switch
            {
                '^' => map.Keys.Where(k => k.y < robotPos.y && k.x == robotPos.x).OrderByDescending(k => k.y).ToList(),
                'v' => map.Keys.Where(k => k.y > robotPos.y && k.x == robotPos.x).OrderBy(k => k.y).ToList(),
                '<' => map.Keys.Where(k => k.x < robotPos.x && k.y == robotPos.y).OrderByDescending(k => k.x).ToList(),
                '>' => map.Keys.Where(k => k.x > robotPos.x && k.y == robotPos.y).OrderBy(k => k.x).ToList(),
                _ => throw new Exception("Unknown direction " + dir.ToString())
            };

            // Check if there is a wall in there
            if (scan.Any(x => map[x] == '#'))
            {
                var pos = scan.IndexOf(scan.First(x => map[x] == '#'));
                scan = scan[0..pos];
            }
            

            // Find first space available
            var space = scan.Where(x => map[x] == '.').FirstOrDefault();
            if (space is null)
                return robotPos;    // No space -> Robot doesn't move

            var index = scan.IndexOf(space);
            for (var i = index; i > 0; i--)
                map[scan[i]] = map[scan[i - 1]];
            map[scan[0]] = '.';

            return scan[0];
        }

        void Display(Coord2D robotPos)
        {
            for (int j = 0; j <= maxY; j++)
            {
                StringBuilder sb = new();
                for (int i = 0; i <= maxX; i++)
                {
                    Coord2D pos = (i, j);
                    sb.Append(pos == robotPos ? '@' : map[pos]);
                }
                Console.WriteLine(sb.ToString());
            }
        }

        int MoveRobot()
        {
            var robotPos = map.Keys.First(x => map[x] == '@');
            map[robotPos] = '.';
            
            
            foreach (var dir in moveSet)
                robotPos = Step(robotPos, dir);
      
            return map.Keys.Where(x => map[x] == 'O').Sum(b => 100 * b.y + b.x);
        }

        public int Solve(int part = 1)
            => MoveRobot();
    }
}
