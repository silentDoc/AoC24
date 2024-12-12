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


        int FindPerimeter(HashSet<Coord2D> area)
            => area.Sum(x => 4 - x.GetNeighbors().Where(n => area.Contains(n)).Count());

        int FindSides(HashSet<Coord2D> area)
        {
            int maxX = map.Keys.Max(k => k.x);
            int maxY = map.Keys.Max(k => k.y);

            var perimeterBlocks = area.Where(a => a.GetNeighbors().Where(n => area.Contains(n)).Count() <4).OrderBy(k => k.y).ThenBy(k => k.x).ToList();

            // sides
            var left = perimeterBlocks.Where(k => !area.Contains(k - (1, 0))).ToList();
            var right = perimeterBlocks.Where(k => !area.Contains(k + (1, 0))).ToList();
            var top = perimeterBlocks.Where(k => !area.Contains(k - (0,1))).ToList();
            var bottom = perimeterBlocks.Where(k => !area.Contains(k + (0, 1))).ToList();

            // left sides
            int sides = 0;
            HashSet<Coord2D> used = new HashSet<Coord2D>();
            foreach (var position in left)
            {
                if (used.Contains(position))
                    continue;
                
                var pos = position;

                while (left.Contains(pos))
                {
                    used.Add(pos);
                    pos += (0, 1);
                }
                pos = position;
                
                while (left.Contains(pos))
                {
                    used.Add(pos);
                    pos -= (0, 1);
                }
                sides++;
            }
            used = new HashSet<Coord2D>();
            foreach (var position in right)
            {
                if (used.Contains(position))
                    continue;

                var pos = position;

                while (right.Contains(pos))
                {
                    used.Add(pos);
                    pos += (0, 1);
                }
                pos = position;

                while (right.Contains(pos))
                {
                    used.Add(pos);
                    pos -= (0, 1);
                }
                sides++;
            }

            used = new HashSet<Coord2D>();
            foreach (var position in top)
            {
                if (used.Contains(position))
                    continue;

                var pos = position;

                while (top.Contains(pos))
                {
                    used.Add(pos);
                    pos += (1, 0);
                }
                pos = position;

                while (top.Contains(pos))
                {
                    used.Add(pos);
                    pos -= (1, 0);
                }
                sides++;
            }

            used = new HashSet<Coord2D>();
            foreach (var position in bottom)
            {
                if (used.Contains(position))
                    continue;

                var pos = position;

                while (bottom.Contains(pos))
                {
                    used.Add(pos);
                    pos += (1, 0);
                }
                pos = position;

                while (bottom.Contains(pos))
                {
                    used.Add(pos);
                    pos -= (1, 0);
                }
                sides++;
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
