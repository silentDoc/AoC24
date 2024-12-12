using AoC24.Common;

namespace AoC24.Day12
{
    internal class GardenChecker
    {
        Dictionary<Coord2D, char> map = new();
        List<HashSet<Coord2D>> areas = new();

        void ParseLine(string line, int row)
            => line.Index().ToList().ForEach(x => map[(x.Index, row)] = x.Item);

        public void ParseInput(List<string> input)
            => input.Index().ToList().ForEach(line => ParseLine(line.Item, line.Index));

        HashSet<Coord2D> DiscoverArea(Coord2D startPos)
        {
            HashSet<Coord2D> area = new();
            Queue<Coord2D> active = new();
            active.Enqueue(startPos);
            char farmChar = map[startPos];

            while (active.Any())
            {
                var pos = active.Dequeue();

                if (area.Contains(pos))
                    continue;
                
                area.Add(pos);

                var neighs = pos.GetNeighbors().Where(x => map.ContainsKey(x) && !area.Contains(x) && map[x] == farmChar).ToList();
                foreach (var neigh in neighs)
                    active.Enqueue(neigh);
            }
            return area;
        }

        // Modified BFS to search only on a list of allowed directions. It finds connected blocks on these
        // directions (sides), restricted to a set of positions
        List<Coord2D> FindSide(Coord2D startPos, List<Coord2D> sidePositions,  List<Coord2D> allowedDirs)
        {
            HashSet<Coord2D> side = new();
            Queue<Coord2D> active = new();
            active.Enqueue(startPos);

            while (active.Any())
            {
                var pos = active.Dequeue();

                if (side.Contains(pos))
                    continue;

                side.Add(pos);
                var neighs = allowedDirs.Select(x => pos + x).Where(x => sidePositions.Contains(x)).ToList();

                foreach (var neigh in neighs)
                    active.Enqueue(neigh);
            }
            return side.ToList();
        }

        int FindPerimeter(HashSet<Coord2D> area)
            => area.Sum(x => 4 - x.GetNeighbors().Where(n => area.Contains(n)).Count());

        int FindSides(HashSet<Coord2D> area)
        {
            int maxX = map.Keys.Max(k => k.x);
            int maxY = map.Keys.Max(k => k.y);
            var sides = 0;
            var perimeterBlocks = area.Where(a => a.GetNeighbors().Where(n => area.Contains(n)).Count() < 4).OrderBy(k => k.y).ThenBy(k => k.x).ToList();

            // sides
            var left = perimeterBlocks.Where(k => !area.Contains(k - (1, 0))).ToList();
            var right = perimeterBlocks.Where(k => !area.Contains(k + (1, 0))).ToList();
            var top = perimeterBlocks.Where(k => !area.Contains(k - (0, 1))).ToList();
            var bottom = perimeterBlocks.Where(k => !area.Contains(k + (0, 1))).ToList();

            List<List<Coord2D>> sideSets = [left, right, top, bottom];
            List<List<Coord2D>> allowedDirs = [[(0, 1), (0, -1)], [(0, 1), (0, -1)], [(1, 0), (-1, 0)], [(1, 0), (-1, 0)]];

            for (int i = 0; i < sideSets.Count; i++)
            {
                var sideSet = sideSets[i];
                var allowed = allowedDirs[i];

                List<Coord2D> used = new List<Coord2D>();
                foreach (var pos in sideSet)
                {
                    if (used.Contains(pos))
                        continue;
                    var side = FindSide(pos, sideSet, allowed);
                    used.AddRange(side);
                    sides++;
                }
            }
            return sides;
        }

        int CheckAreas(int part =1)
        {
            int maxX = map.Keys.Max(k => k.x);
            int maxY = map.Keys.Max(k => k.y);

            for (int x = 0; x <= maxX; x++)
                for (int y = 0; y <= maxY; y++)
                {
                    Coord2D pos = (x, y);
                       
                    if (areas.Any(a => a.Contains(pos)))
                        continue;

                    var newArea = DiscoverArea(pos);
                    areas.Add(newArea);
                }

            return part == 1 ? areas.Select(a => a.Count() * FindPerimeter(a)).Sum()
                             : areas.Select(a => a.Count() * FindSides(a)).Sum();
        }

        public int Solve(int part = 1)
            => CheckAreas(part);
    }
}
