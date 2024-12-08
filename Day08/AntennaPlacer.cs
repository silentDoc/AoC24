using AoC24.Common;

namespace AoC24.Day08
{
    internal class AntennaPlacer
    {
        Dictionary<Coord2D, char> map = new();
        HashSet<Coord2D> antinodes = new();

        void ParseLine((int index, string item) element)
        {
            for (int i = 0; i < element.item.Length; i++)
                map[(i, element.index)] = element.item[i];
        }

        public void ParseInput(List<string> input)
            => input.Index().ToList().ForEach(x => ParseLine(x));

        void BuildAntinode(char antenna)
        {
            var locations = map.Keys.Where(x => map[x] == antenna).ToList();
            
            if (locations.Count() == 1)
                return;

            for (int i = 0; i < locations.Count() - 1; i++)
                for (int j = i + 1; j < locations.Count(); j++)
                {
                    var dif = locations[j] - locations[i];
                    var antinode1 = locations[j] + dif;
                    var antinode2 = locations[i] - dif;
                    
                    if (map.ContainsKey(antinode1))
                        antinodes.Add(antinode1);
                    if (map.ContainsKey(antinode2))
                        antinodes.Add(antinode2);
                }
        }

        void BuildAntinodeLine(char antenna)
        {
            var locations = map.Keys.Where(x => map[x] == antenna).ToList();

            if (locations.Count() == 1)
                return;

            for (int i = 0; i < locations.Count() - 1; i++)
                for (int j = i + 1; j < locations.Count(); j++)
                {
                    var dif = locations[j] - locations[i];

                    var antinode1 = locations[j] + dif;
                    var antinode2 = locations[i] - dif;

                    antinodes.Add(locations[j]);
                    antinodes.Add(locations[i]);

                    while (map.ContainsKey(antinode1))
                    {
                        antinodes.Add(antinode1);
                        antinode1 += dif;
                    }

                    while (map.ContainsKey(antinode2))
                    {
                        antinodes.Add(antinode2);
                        antinode2 -= dif;
                    }
                }
        }

        int FindAntinodes(int part = 1)
        {
            var antennas = map.Values.Where(x => x != '.').ToHashSet();
            foreach (var antenna in antennas)
                if(part == 1)
                    BuildAntinode(antenna);
                else
                    BuildAntinodeLine(antenna);

            return antinodes.Count();
        }



        public int Solve(int part = 1)
            => FindAntinodes(part);
    }
}
