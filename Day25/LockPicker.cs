using AoC24.Common;

namespace AoC24.Day25
{
    enum SType
    { 
        Key, 
        Lock
    }

    class Schematic
    {
        Dictionary<Coord2D, char> map = new();
        public SType Type;
        public List<int> Heights = new();

        int maxY = -1;

        public Schematic(List<string> input)
        {
            for (int j = 0; j < input.Count; j++)
                for (int i = 0; i < input[0].Length; i++)
                    map[(i, j)] = input[j][i];

            maxY = input.Count;
            Type = (map[(0, 0)] == '#') ? SType.Lock: SType.Key;

            // Todo - learn to use linq groupby properly
            //Heights = map.Keys.GroupBy(k => k.x, k => k, (xCoord, pos) => new { Key = xCoord, count = pos.Count() });

            var columns = Enumerable.Range(0, input[0].Length).ToList();
            foreach (var col in columns)
            {
                var count = map.Keys.Where(k => k.x == col).Count(k => map[k] == '#');
                Heights.Add(count);
            }
        }

        public bool NoOverlap(Schematic other)
        {
            if (other.Type == this.Type)
                return false;

            return Heights.Zip(other.Heights, (a, b) => a + b).All(x => x <= maxY);
        }

        public bool Fits(Schematic other)
        {
            if (other.Type == this.Type)
                return false;

            return Heights.Zip(other.Heights, (a,b) => a + b).All(x => x == maxY);
        }
    }

    internal class LockPicker
    {
        List<Schematic> schematics = new();
        public void ParseInput(List<string> input)
        {
            var schms = ParseUtils.SplitBy(input, "");
            schms.ForEach(x => schematics.Add(new(x)));
        }

        int FindGoodKeys()
        {
            var Keys  = schematics.Where(s => s.Type == SType.Key).ToList();
            var Locks = schematics.Where(s => s.Type == SType.Lock).ToList();
            return Keys.Sum(k => Locks.Where(l => l.NoOverlap(k) == true).Count());
        }

        public int Solve(int part = 1)
            => FindGoodKeys();
    }
}
