using AoC24.Common;

namespace AoC24.Day10
{
    internal class LavaHiker
    {
        Dictionary<Coord2D, int> map = new();

        void ParseLine(string line, int row)
            => line.Index().ToList().ForEach(x => map[(x.Index, row)] = int.Parse(x.Item.ToString()));

        public void ParseInput(List<string> input)
            => input.Index().ToList().ForEach(line => ParseLine(line.Item, line.Index));

        int TraverseMapPaths(Coord2D start, int part)
        {
            HashSet<Coord2D> visited = new();
            HashSet<List<Coord2D>> topPaths = new();
            Queue<List<Coord2D>> active = new();

            active.Enqueue([start]);

            while (active.Any())
            {
                var path = active.Dequeue();
                var pos = path.Last();

                if (map[pos] == 9)
                    topPaths.Add(path);

                var eval = pos.GetNeighbors().Where(x => map.ContainsKey(x) && map[pos] == map[x] - 1).ToList();

                foreach (var neigh in eval)
                    active.Enqueue([..path, neigh]);
            }

            return part == 1 ? topPaths.Select(x => x.Last()).Distinct().Count()
                             : topPaths.Count();
        }

        int FindScores(int part = 1)
        {
            var startPositions = map.Keys.Where(x => map[x] == 0);
            return startPositions.Sum(x => TraverseMapPaths(x, part));
        }

        public int Solve(int part = 1)
            => FindScores(part);
    }
}
