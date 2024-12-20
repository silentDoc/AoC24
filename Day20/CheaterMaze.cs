using AoC24.Common;

namespace AoC24.Day20
{
    internal class CheaterMaze
    {
        Dictionary<Coord2D, char> map = new();

        public void ParseInput(List<string> input)
           => input.Index().ToList().ForEach(x => ParseLine(x.Index, x.Item));

        void ParseLine(int row, string line)
            => line.Index().ToList().ForEach(x => map[(x.Index, row)] = x.Item);

        int SolvePart1()
        {
            bool solved = false;
            List<Coord2D> finalTrail = [];
            int finalCost = -1;

            HashSet<Coord2D> visited = new();
            Queue<(Coord2D pos, int cost, List<Coord2D> trail)> active = new();

            Coord2D current = map.Keys.First(x => map[x] == 'S');
            Coord2D endPos = map.Keys.First(x => map[x] == 'E');
            active.Enqueue((current, 0, [current]));

            // We solve the maze first, but we do not cheat - we keep the trail though so we do not have
            // to run a BFS for every cheating account. 
            while (active.Any())
            {
                var element = active.Dequeue();
                var currentPos = element.pos;
                var currentCost = element.cost;
                var currentTrail = element.trail;

                if (visited.Contains(currentPos))
                    continue;

                visited.Add(currentPos);

                if (currentPos == endPos)
                {
                    finalCost = currentCost;
                    finalTrail = currentTrail;
                    solved = true;
                    break;
                }

                var neighs = currentPos.GetNeighbors().Where(x => map.ContainsKey(x) && map[x] != '#' && !visited.Contains(x)).ToList();

                foreach (var neigh in neighs)
                    active.Enqueue((neigh, currentCost + neigh.Manhattan(currentPos), [..currentTrail, neigh]));
            }
            
            if(!solved)
                return -1;

            // We have the trail, we have to see how many cheats in the trail save us cost. The index of each trail position its is cost
            Dictionary<int, int> savings = new();

            for (int i = 0; i < finalTrail.Count; i++)
            { 
                var trailPos = finalTrail[i];
                var cheatPos = trailPos.GetNeighbors(2).Where(x => finalTrail.Contains(x) && finalTrail.IndexOf(x) > i).ToList();

                foreach (var cheat in cheatPos)
                {
                    var saved = finalTrail.IndexOf(cheat) - i - 2;
                    if (!savings.ContainsKey(saved))
                        savings[saved] = 1;
                    else
                        savings[saved]++;
                }
            }

            return savings.Keys.Where(k => k>=100).Sum(k => savings[k]);
        }

        public int Solve(int part = 1)
            => SolvePart1();
    }
}
