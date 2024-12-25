using AoC24.Common;

namespace AoC24.Day25
{
    class Schematic
    {
        Dictionary<Coord2D, char> map = new();
        public bool IsKey;
        public List<int> Heights = new();
        int maxY = -1;

        public Schematic(List<string> input)
        {
            for (int j = 0; j < input.Count; j++)
                for (int i = 0; i < input[0].Length; i++)
                    map[(i, j)] = input[j][i];

            maxY = input.Count;
            IsKey = (map[(0, 0)] == '.');

            for(int col = 0; col < input[0].Length; col++)
                Heights.Add( map.Keys.Where(k => k.x == col).Count(k => map[k] == '#') );
        }

        public bool NoOverlap(Schematic other)
            => (other.IsKey == this.IsKey) ? false
                                           : Heights.Zip(other.Heights, (a, b) => a + b).All(x => x <= maxY);
    }

    internal class LockPicker
    {
        List<Schematic> schematics = new();
        public void ParseInput(List<string> input)
        {
            var schms = ParseUtils.SplitBy(input, "");
            schms.ForEach(x => schematics.Add(new(x)));
        }

        int FindPairs()
        {
            var Keys  = schematics.Where(s => s.IsKey).ToList();
            var Locks = schematics.Where(s => !s.IsKey).ToList();
            return Keys.Sum(k => Locks.Where(l => l.NoOverlap(k) == true).Count());
        }

        public int Solve(int part = 1)
            => FindPairs();
    }
}
