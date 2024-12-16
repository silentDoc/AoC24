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
                (0, -1) => [Direction.North, Direction.West,  Direction.East],    // N -> N, W, E
                (1,  0) => [Direction.East,  Direction.North, Direction.South],    // E -> E, N, S
                (0,  1) => [Direction.South, Direction.East,  Direction.West],     // S -> S, E, W
                (-1, 0) => [Direction.West,  Direction.South, Direction.North],   // W -> W, S, N
            };

        int SolveMap(int part = 1)
        {
            int bestCost = int.MaxValue;

            Dictionary<(Coord2D pos, Coord2D facing), int> visitedCosts = new();
            List<(List<Coord2D> trail, int score)> trailsToEnd = new();
            Queue<(Coord2D pos, Coord2D facing, int cost, List<Coord2D> trail)> active = new();

            var startPos = map.Keys.First(x => map[x] == 'S');
            var endPos = map.Keys.First(x => map[x] == 'E');
            active.Enqueue((startPos, Direction.East, 0, [startPos]));

            while (active.Any())
            {  
                // For each element in the queue, we consider position, facing, cost and accumulated trail
                var element = active.Dequeue();    
                var currentPos = element.pos;
                var currentFacing = element.facing;
                var currentCost = element.cost;
                var currentTrail = element.trail;

                if (currentCost > bestCost)     // Improves performance
                    continue;
              
                if (visitedCosts.ContainsKey((currentPos, currentFacing)))
                    if (currentCost > visitedCosts[(currentPos, currentFacing)])
                        continue;
                
                visitedCosts[(currentPos, currentFacing)] = currentCost;

                if (currentPos == endPos)
                {
                    trailsToEnd.Add(([..currentTrail,currentPos], currentCost));
                    if (currentCost < bestCost)
                        bestCost = currentCost;
                    continue;
                }

                var nextFacings = GetNextDirs(currentFacing);
                var validFacings = nextFacings.Select(k => map.ContainsKey(currentPos + k) && map[currentPos + k] != '#').ToList();

                for(int i=0; i<validFacings.Count; i++)
                {
                    if (!validFacings[i])
                        continue;

                    var costInc = nextFacings[i] == currentFacing ? 1 : 1001;      // 1001 = 1000 to turn and 1 to advance on that direction
                    List<Coord2D> newTrail = [..currentTrail, currentPos + nextFacings[i]];
                    active.Enqueue((currentPos+ nextFacings[i], nextFacings[i], currentCost + costInc, newTrail));
                }
            }

            return (part == 1) ? visitedCosts.Keys.Where(x => x.pos == endPos).Min(x => visitedCosts[x])
                               : trailsToEnd.Where(t => t.score == bestCost).SelectMany(x => x.trail).ToHashSet().Count();
        }

        public int Solve(int part = 1)
            => SolveMap(part);
    }
}
