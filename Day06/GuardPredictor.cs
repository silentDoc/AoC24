using AoC24.Common;

namespace AoC24.Day06
{
    public static class Direction
    {
        public static Coord2D Up = new(0, -1);
        public static Coord2D Down = new(0, 1);
        public static Coord2D Left = new(-1, 0);
        public static Coord2D Right = new(1, 0);
    }

    internal class GuardPredictor
    {
        Dictionary<Coord2D, char> map = new();

        void ParseLine((int index, string item) element)
        { 
            for(int i = 0; i<element.item.Length; i++)
                map[(i, element.index)] = element.item[i];
        }
            
        public void ParseInput(List<string> input)
            => input.Index().ToList().ForEach(x => ParseLine(x));

        Coord2D TurnRight(Coord2D currentDirection)
            => currentDirection switch
            {
                (0, -1) => Direction.Right,
                (-1, 0) => Direction.Up,
                (0, 1) => Direction.Left,
                (1, 0) => Direction.Down,
                _ => throw new Exception("Invalid direction " + currentDirection.ToString())
            };

        HashSet<Coord2D> GuardPositions()
        {
            Coord2D currentPos = map.Keys.First(x => map[x] == '^');
            Coord2D currentDir = Direction.Up;
            HashSet<Coord2D> visited = new();

            while (map.ContainsKey(currentPos))
            {
                visited.Add(currentPos);
                var nextPos = currentPos + currentDir;
                
                if (!map.ContainsKey(nextPos))
                    break;

                if (map[nextPos] == '#')
                    currentDir = TurnRight(currentDir);
                else
                    currentPos = nextPos;
            }

            return visited;
        }

        bool InALoop(Coord2D obstruction)
        {
            // Slight modification of part 1, could use a single method but left separate for clarity
            Coord2D currentPos = map.Keys.First(x => map[x] == '^');
            Coord2D currentDir = Direction.Up;
            HashSet<string> visited = new();
            map[obstruction] = '#';

            while (map.ContainsKey(currentPos))
            {
                if (!visited.Add(currentPos.ToString() + ";" + currentDir.ToString()))
                {
                    map[obstruction] = '.';
                    return true;
                }

                var nextPos = currentPos + currentDir;

                if (!map.ContainsKey(nextPos))
                    break;

                if (map[nextPos] == '#')
                    currentDir = TurnRight(currentDir);
                else
                    currentPos = nextPos;
            }
            map[obstruction] = '.';
            return false;
        }

        int FindNumberPossibleObstructions()
        {
            var startPos = map.Keys.First(x => map[x] == '^');
            return GuardPositions().Where(x => x != startPos && InALoop(x)).Count();
        }

        public int Solve(int part = 1)
            => part == 1 ? GuardPositions().Count() : FindNumberPossibleObstructions();
    }
}
