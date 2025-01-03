﻿using AoC24.Common;

namespace AoC24.Day20
{
    internal class CheaterMaze
    {
        Dictionary<Coord2D, char> map = new();

        public void ParseInput(List<string> input)
           => input.Index().ToList().ForEach(x => ParseLine(x.Index, x.Item));

        void ParseLine(int row, string line)
            => line.Index().ToList().ForEach(x => map[(x.Index, row)] = x.Item);

        int SolveMaze(int part = 1)
        {
            List<Coord2D> path = [];
            Dictionary<Coord2D, int> costsLookup = new();
            Queue<(Coord2D pos, int cost, List<Coord2D> path)> active = new();

            Coord2D current = map.Keys.First(x => map[x] == 'S');
            Coord2D endPos = map.Keys.First(x => map[x] == 'E');
            active.Enqueue((current, 0, [current]));

            // We solve the maze first, but we do not cheat - we keep the trail though so we do not have
            // to run a BFS for every cheating attempt in different positions. 
            // Another thing I realized is that the puzzle today seems to have only one path from S to E, so 
            // the best way is indeed to get that trail and work the cheats on the positions afterwards
            // -- maybe we could even skip the bfs ? 
            while (active.Any())
            {
                var element = active.Dequeue();
                var currentPos = element.pos;
                var currentCost = element.cost;
                path = element.path;

                if (costsLookup.ContainsKey(currentPos))
                    continue;

                costsLookup[currentPos] = currentCost;

                if (currentPos == endPos)
                    break;

                var neighs = currentPos.GetNeighbors().Where(x => map.ContainsKey(x) && map[x] != '#' && !costsLookup.ContainsKey(x)).ToList();

                foreach (var neigh in neighs)
                    active.Enqueue((neigh, currentCost + neigh.Manhattan(currentPos), [..path, neigh]));
            }

            // We have the trail, we have to see how many cheats in the trail save us cost. The index of each trail position its is cost
            // The first approach on part2 took more than 30 minutes in my machine -- I will refactor to accelerate that stuff
            // Update : I did adding the lookup dictionary and cutting the candidates to explore, and running a parallel for. Now it flies
            Dictionary<int, int> savings = new();
            object lockObj = new();

            Parallel.For(0, path.Count, (i, state) =>
            {
                List<Coord2D> explore = part == 1 ? path : path[(i + 1)..];

                var trailPos = path[i];
                var cheatPos = part == 1 ? trailPos.GetNeighbors(2).Where(x => costsLookup.ContainsKey(x)).ToHashSet()
                                         : explore.Where(x => trailPos.Manhattan(x) <= 20).ToHashSet();
                
                var savedCost = part == 1 ? cheatPos.Select(x => costsLookup[x] - i - 2)
                                          : cheatPos.Select(x => costsLookup[x] - i - trailPos.Manhattan(x));

                lock (lockObj)
                {
                    foreach (var el in savedCost)
                        if (!savings.ContainsKey(el))
                            savings[el] = 1;
                        else
                            savings[el]++;
                }
            });

            return savings.Keys.Where(k => k>=100).Sum(k => savings[k]);
        }

        public int Solve(int part = 1)
            => SolveMaze(part);
    }
}
