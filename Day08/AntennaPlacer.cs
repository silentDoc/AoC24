using AoC24.Common;

namespace AoC24.Day08
{
    internal class AntennaPlacer
    {
        Dictionary<Coord2D, char> map = new();
        HashSet<Coord2D> antinodes = new();

        void ParseLine((int index, string item) line)
        {
            for (int i = 0; i < line.item.Length; i++)
                map[(i, line.index)] = line.item[i];
        }

        public void ParseInput(List<string> input)
            => input.Index().ToList().ForEach(x => ParseLine(x));

        void AddAntiNodes(char antenna, int part = 1)
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

                    if (part == 1)
                    {
                        if (map.ContainsKey(antinode1))
                            antinodes.Add(antinode1);
                        if (map.ContainsKey(antinode2))
                            antinodes.Add(antinode2);
                    }
                    else
                    {

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
        }

        int FindAntinodes(int part = 1)
        {
            var antennas = map.Values.Where(x => x != '.').ToHashSet();
            foreach (var antenna in antennas)
                AddAntiNodes(antenna, part);

            return antinodes.Count();
        }

        public int Solve(int part = 1)
            => FindAntinodes(part);
    }
}
