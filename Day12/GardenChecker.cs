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


        int GetAreaPerimeter(HashSet<Coord2D> area)
            => area.Sum(x => 4 - x.GetNeighbors().Where(n => area.Contains(n)).Count());

        int CheckAreas()
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

            var positions = areas.SelectMany(x => x).ToList();
            var missing = map.Keys.Where(x => !positions.Contains(x)).ToList();

            return areas.Select(a => a.Count() * GetAreaPerimeter(a)).Sum();
        }

        public int Solve(int part = 1)
            => CheckAreas();
    }
}
