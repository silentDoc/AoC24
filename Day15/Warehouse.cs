using AoC24.Common;
using System.Text;

namespace AoC24.Day15
{
    record Box(Coord2D left, Coord2D right)
    {
        public HashSet<Box> GetPushedBoxes(Dictionary<Coord2D, char> map, char dir)
        {
            List<Box> res = [];

            Coord2D next = dir switch
            {
                '^' => (0, -1),
                'v' => (0, 1),
                _ => throw new Exception("Unknown direction " + dir.ToString())
            };

            var nextLeft = left + next;
            var nextRight = right + next;

            if (map[nextLeft] == ']')
                res.Add(new Box(nextLeft-(1,0), nextLeft));
            if (map[nextLeft] == '[')
                res.Add(new Box(nextLeft, nextRight));
            if (map[nextRight] == '[')
                res.Add(new Box(nextRight, nextRight+(1,0)));

            var additionalBoxes = res.SelectMany(x => x.GetPushedBoxes(map, dir)).ToHashSet();

            HashSet<Box> retVal = [..res,..additionalBoxes];
            return retVal;
        }

        public bool CanMove(Dictionary<Coord2D, char> map, char dir, HashSet<Coord2D> movingPositions)
        {
            // We consider only up and down because left and right is exactly as part 1
            var scanLeft = dir switch
            {
                '^' => map.Keys.Where(k => k.y < left.y && k.x == left.x).OrderByDescending(k => k.y).ToList(),
                'v' => map.Keys.Where(k => k.y > left.y && k.x == left.x).OrderBy(k => k.y).ToList(),
                _ => throw new Exception("Unknown direction " + dir.ToString())
            };

            var scanRight = dir switch
            {
                '^' => map.Keys.Where(k => k.y < right.y && k.x == right.x).OrderByDescending(k => k.y).ToList(),
                'v' => map.Keys.Where(k => k.y > right.y && k.x == right.x).OrderBy(k => k.y).ToList(),
                _ => throw new Exception("Unknown direction " + dir.ToString())
            };

            Coord2D offset = dir switch
            {
                '^' => (0, -1),
                'v' => (0, 1),
                _ => throw new Exception("Unknown direction " + dir.ToString())
            };

            var nextL = left + offset;
            var nextR = right + offset;

            bool canMoveL = movingPositions.Contains(nextL);    // We assume that we will be able to move the box onto a position of 
            bool canMoveR = movingPositions.Contains(nextR);    // a box that is also moving in this iteration.

            // Check if there is a wall in there, and if so, consider only the cells until the wall
            if (scanLeft.Any(x => map[x] == '#'))
            {
                var pos = scanLeft.IndexOf(scanLeft.First(x => map[x] == '#'));
                scanLeft = scanLeft[0..pos];
            }

            if (scanRight.Any(x => map[x] == '#'))
            {
                var pos = scanRight.IndexOf(scanRight.First(x => map[x] == '#'));
                scanRight = scanRight[0..pos];
            }

            // Find first space available
            var spaceLeft = scanLeft.Where(x => map[x] == '.').FirstOrDefault();
            canMoveL = canMoveL || !(spaceLeft is null);

            var spaceRight = scanRight.Where(x => map[x] == '.').FirstOrDefault();
            canMoveR = canMoveR || !(spaceRight is null);

            return canMoveL && canMoveR;
        }
    }

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
            var scan = GetScan(robotPos, dir);

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

        List<Coord2D> GetScan(Coord2D robotPos, char dir)
            => dir switch
               {
                   '^' => map.Keys.Where(k => k.y < robotPos.y && k.x == robotPos.x).OrderByDescending(k => k.y).ToList(),
                   'v' => map.Keys.Where(k => k.y > robotPos.y && k.x == robotPos.x).OrderBy(k => k.y).ToList(),
                   '<' => map.Keys.Where(k => k.x < robotPos.x && k.y == robotPos.y).OrderByDescending(k => k.x).ToList(),
                   '>' => map.Keys.Where(k => k.x > robotPos.x && k.y == robotPos.y).OrderBy(k => k.x).ToList(),
                   _ => throw new Exception("Unknown direction " + dir.ToString())
               };

        Coord2D StepPart2(Coord2D robotPos, char dir)
        {
            Coord2D offset = dir switch
            {
                '^' => (0, -1),
                'v' => (0, 1),
                '<' => (-1, 0),
                '>' => (1, 0),
                _ => throw new Exception("Unknown direction " + dir.ToString())
            };

            var next = robotPos + offset;

            // If we move into a wall, we don't move. If we move into a space, we can move
            if (map[next] == '#')
                return robotPos;
            
            if (map[next] == '.')
                return next;

            // We found a box - but if it is Left / Right is like part 1
            if (dir == '<' || dir == '>')
                return Step(robotPos, dir);

            // Up and down is where it gets interesting
            var left = map[next] == '[' ? next : next - (1, 0);
            var right = map[next] == ']' ? next : next + (1, 0);
            var firstBox = new Box(left, right);

            // Find all the boxes that will be moved in this iteration
            HashSet<Box> boxesAffected = [firstBox,..firstBox.GetPushedBoxes(map, dir)];
            
            // Retrieve all the positions of the boxes to move
            var leftPositions = boxesAffected.Select(b => b.left);
            var rightPositions = boxesAffected.Select(b => b.right);
            var movingPositions = leftPositions.Union(rightPositions).ToHashSet();

            // A single box unable to move can prevent the whole set to move
            if (boxesAffected.Any(x => !x.CanMove(map, dir, movingPositions)))
                return robotPos;

            // Move all the boxes 1 step in the direction
            var newLeftPositions = boxesAffected.Select(b => b.left + offset).ToHashSet();
            var newRightPositions = boxesAffected.Select(b => b.right + offset).ToHashSet();

            foreach (var pos in movingPositions)
                map[pos] = '.';
            foreach (var pos in newLeftPositions)
                map[pos] = '[';
            foreach (var pos in newRightPositions)
                map[pos] = ']';

            return next;
        }

        void ExpandMap()
        {
            Dictionary<Coord2D, char> newMap = new();

            for (int j = 0; j <= maxY; j++)
                for (int i = 0; i <= maxX; i++)
                {
                    newMap[(i * 2, j)]     = map[(i, j)] == '@' ? '@' : map[(i, j)] == 'O' ? '[' : map[(i, j)];
                    newMap[(i * 2 + 1, j)] = map[(i, j)] == '@' ? '.' : map[(i, j)] == 'O' ? ']' : map[(i, j)];
                }
            
            map = newMap;
            maxX = map.Keys.Max(k => k.x);
            maxY = map.Keys.Max(k => k.y);
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

        int MoveRobot(int part = 1)
        {
            if (part == 2)
                ExpandMap();
            
            var robotPos = map.Keys.First(x => map[x] == '@');
            map[robotPos] = '.';

            foreach (var dir in moveSet)
                robotPos = part == 1 ? Step(robotPos, dir) : StepPart2(robotPos, dir);

            Display(robotPos);

            return part == 1 ? map.Keys.Where(x => map[x] == 'O').Sum(b => 100 * b.y + b.x)
                             : map.Keys.Where(x => map[x] == '[').Sum(b => 100 * b.y + b.x);
        }

        public int Solve(int part = 1)
            => MoveRobot(part);
    }
}
