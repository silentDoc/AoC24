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

        HashSet<Coord2D> FindConnectedPositions(Coord2D startPos, HashSet<Coord2D> allowedPositions, HashSet<Coord2D> allowedDirs)
        {
            HashSet<Coord2D> positionSet = new();
            Queue<Coord2D> active = new();
            char farmChar = map[startPos];
            active.Enqueue(startPos);

            while (active.Any())
            {
                var pos = active.Dequeue();

                if (positionSet.Contains(pos))
                    continue;

                positionSet.Add(pos);
                var neighs = allowedDirs.Select(x => pos + x).Where(x => allowedPositions.Contains(x) && map[x] == farmChar).ToList();

                foreach (var neigh in neighs)
                    active.Enqueue(neigh);
            }
            return positionSet;
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
                var sideSet = sideSets[i].ToHashSet();
                var allowed = allowedDirs[i].ToHashSet();

                List<Coord2D> used = new List<Coord2D>();
                foreach (var pos in sideSet)
                {
                    if (used.Contains(pos))
                        continue;
                    var side = FindConnectedPositions(pos, sideSet, allowed);
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
            var allowedPositions = map.Keys.ToHashSet();
            var allowedDirections = new Coord2D(0, 0).GetNeighbors().ToHashSet(); 

            for (int x = 0; x <= maxX; x++)
                for (int y = 0; y <= maxY; y++)
                {
                    Coord2D pos = (x, y);
                       
                    if (areas.Any(a => a.Contains(pos)))
                        continue;

                    var newArea = FindConnectedPositions(pos, allowedPositions, allowedDirections);
                    areas.Add(newArea);
                }

            return part == 1 ? areas.Select(a => a.Count() * FindPerimeter(a)).Sum()
                             : areas.Select(a => a.Count() * FindSides(a)).Sum();
        }

        public int Solve(int part = 1)
            => CheckAreas(part);
    }
}
