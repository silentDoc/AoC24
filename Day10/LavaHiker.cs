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

        int TraverseMap(Coord2D start)
        {
            HashSet<Coord2D> visited = new();
            HashSet<Coord2D> tops = new();

            Queue<Coord2D> active = new();
            active.Enqueue(start);

            while (active.Any())
            {
                var pos = active.Dequeue();
                
                if (map[pos] == 9)
                    tops.Add(pos);

                var eval = pos.GetNeighbors().Where(x => map.ContainsKey(x) && map[pos] == map[x] - 1).ToList();
                eval.ForEach(active.Enqueue);
            }
            return tops.Count();
        }


        int FindScores()
        {
            var startPositions = map.Keys.Where(x => map[x] == 0);
            return startPositions.Sum(x => TraverseMap(x));
        }

        public int Solve(int part = 1)
            => FindScores();
    }
}
