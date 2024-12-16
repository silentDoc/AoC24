using AoC24.Common;

namespace AoC24.Day16
{
    static class Direction
    {
        public static readonly Coord2D North = (0, -1);
        public static readonly Coord2D East  = (1, 0);
        public static readonly Coord2D South = (0, 1);
        public static readonly Coord2D West  = (-1, 0);
    }

    internal class MazeRunner
    {
        Dictionary<Coord2D, char> map = new();
  
        public void ParseInput(List<string> input)
            => input.Index().ToList().ForEach(element => ParseLine(element.Index, element.Item));

        void ParseLine(int row, string line)
            => line.Index().ToList().ForEach(c => map[(c.Index, row)] = c.Item);

        public List<Coord2D> GetNextDirs(Coord2D currentFacing)
            => currentFacing switch
            {
                (0, -1) => [Direction.North, Direction.West, Direction.East],    // N -> N, W, E
                (1, 0) => [Direction.East, Direction.North, Direction.South],    // E -> E, N, S
                (0, 1) => [Direction.South, Direction.East, Direction.West],     // S -> S, E, W
                (-1, 0) => [Direction.West, Direction.South, Direction.North],   // W -> W, S, N
            };

        int BFS(Coord2D startPos, Coord2D endPos, int part = 1)
        {
            int bestCost = int.MaxValue;

            Dictionary<(Coord2D pos, Coord2D facing), int> visitedCosts = new();
            Queue<(Coord2D pos, Coord2D facing, int cost, List<Coord2D> trail)> active = new();
            List<(List<Coord2D> trail, int score)> trailsToEnd = new();

            char farmChar = map[startPos];
            active.Enqueue((startPos, Direction.East, 0, [startPos]));

            while (active.Any())
            {
                var element = active.Dequeue();
                var pos = element.pos;
                var facing = element.facing;
                var cost = element.cost;
                var trail = element.trail;

                if (cost > bestCost)
                    continue;
              
                if (visitedCosts.ContainsKey((pos, facing)))
                    if (cost > visitedCosts[(pos, facing)])
                        continue;
                
                visitedCosts[(pos, facing)] = cost;

                if (pos == endPos)
                {
                    trailsToEnd.Add(([..trail,pos], cost));
                    if (cost < bestCost)
                        bestCost = cost;
                    continue;
                }

                var nextFacings = GetNextDirs(facing);
                var validFacings = nextFacings.Select(k => map.ContainsKey(pos + k) && map[pos + k] != '#').ToList();

                for(int i=0; i<validFacings.Count; i++)
                {
                    if (!validFacings[i])
                        continue;

                    var costInc = nextFacings[i] == facing ? 1 : 1001;      // 1001 = 1000 to turn and 1 to advance on that direction
                    List<Coord2D> newTrail = [..trail, pos + nextFacings[i]];
                    active.Enqueue((pos+ nextFacings[i], nextFacings[i], cost + costInc, newTrail));
                }
            }

            if (part == 2)
            {
                var positions = trailsToEnd.Where(t => t.score == bestCost).SelectMany(x => x.trail).ToHashSet();
                return positions.Count();
            }

            var endPositionKeys = visitedCosts.Keys.Where(x => x.pos == endPos);
            return endPositionKeys.Select(x => visitedCosts[x]).Min();
        }

        int SolveMaze(int part)
        {
            var start = map.Keys.First(x => map[x] == 'S');
            var end = map.Keys.First(x => map[x] == 'E');
            return BFS(start, end, part);
        }

        public int Solve(int part = 1)
            => SolveMaze(part);
    }
}
