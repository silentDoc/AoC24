using AoC24.Common;

namespace AoC24.Day10
{
    internal class LavaHiker
    {
        Dictionary<Coord2D, int> map = new();

        void ParseLine((int index, string item) line)
        {
            for(int i =0; i<line.item.Length; i++) 
                map[(i, line.index)] = int.Parse(line.item[i].ToString());
        }

        public void ParseInput(List<string> input)
            => input.Index().ToList().ForEach(line => ParseLine(line));

        int TraverseMapPaths(Coord2D start, int part)
        {
            HashSet<Coord2D> visited = new();
            HashSet<List<Coord2D>> topPaths = new();

            Queue<(Coord2D, List<Coord2D> Path)> active = new();
            active.Enqueue((start, []));

            while (active.Any())
            {
                var (pos, path) = active.Dequeue();

                if (map[pos] == 9)
                    topPaths.Add([..path,pos]);

                var eval = pos.GetNeighbors().Where(x => map.ContainsKey(x) && map[pos] == map[x] - 1).ToList();

                foreach (var neigh in eval)
                    active.Enqueue((neigh, [.. path, pos]));
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
