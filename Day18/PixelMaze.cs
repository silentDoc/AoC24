using AoC24.Common;
using System.Text;

namespace AoC24.Day18
{
    internal class PixelMaze
    {
        Dictionary<Coord2D, char> map = new();
        List<Coord2D> fallingPixels   = new();
        int maxDim = 70;
        //int maxDim = 6;

        public void ParseInput(List<string> input)
           => input.ForEach(ParseLine);

        void ParseLine(string line)
        {
            var v = line.Split(',').Select(int.Parse).ToList();
            fallingPixels.Add((v[0], v[1]));
        }

        void Display()
        {
            for (int i = 0; i <= maxDim; i++)
            {
                StringBuilder sb = new("");
                for (int j = 0; j <= maxDim; j++)
                    sb.Append(map[(i, j)]);

                Console.WriteLine(sb.ToString());
            }
        }

        int SolveMaze(int numBlocks)
        {
            // Prep the map
            for (int i = 0; i <= maxDim; i++)
                for (int j = 0; j <= maxDim; j++)
                    map[(i, j)] = '.';
            
            
            for(int i = 0; i<numBlocks; i++)
                map[fallingPixels[i]] = '#';

            Display();

            HashSet<Coord2D> visited = new();
            Queue<(Coord2D pos, int cost)> active = new();

            Coord2D current = (0, 0);
            Coord2D endPos = (maxDim, maxDim);
            active.Enqueue((current, 0));

            while (active.Any())
            {
                var element = active.Dequeue();
                var currentPos = element.pos;
                var currentCost = element.cost;

                if (visited.Contains(currentPos))
                    continue;

                visited.Add(currentPos);

                if (currentPos == endPos)
                    return currentCost;

                var neighs = currentPos.GetNeighbors().Where(x => map.ContainsKey(x) && map[x] != '#' && !visited.Contains(x)).ToList();

                foreach (var neigh in neighs)
                    active.Enqueue((neigh, currentCost + 1));
            }
            return -1;
        }

        public int Solve(int part = 1)
            => SolveMaze(1024);
    }
}
