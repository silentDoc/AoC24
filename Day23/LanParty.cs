namespace AoC24.Day23
{
    internal class LanParty
    {
        Dictionary<string, HashSet<string>> conns = new();

        void ParseLine(string line)
        {
            var comps = line.Split("-");
            if (!conns.ContainsKey(comps[0]))
                conns[comps[0]] = new();
            if (!conns.ContainsKey(comps[1]))
                conns[comps[1]] = new();
            conns[comps[0]].Add(comps[1]);
            conns[comps[1]].Add(comps[0]);
        }

        public void ParseInput(List<string> input)
            => input.ForEach(ParseLine);

        int Find3Groups()
        {
            HashSet<string> groups = new();

            foreach (var computer in conns.Keys)
            {
                var connectedComputers = conns[computer];
                foreach (var neighbor in connectedComputers)
                {
                    var commonPCs = connectedComputers.Intersect(conns[neighbor]).ToHashSet();
                    
                    if (commonPCs.Count == 0)
                        continue;

                    var possibleGroups = commonPCs.Select(x => new List<string>([computer, neighbor, x])).ToList();
                    var strPossibleGroups = possibleGroups.Select(x => string.Join('-', x.OrderBy(y => y))).ToList();
                    strPossibleGroups.ForEach(x => groups.Add(x));
                }
            }

            var listGroups = groups.ToList().OrderBy(x => x).ToList();
            return listGroups.Count(x => x.IndexOf("-t") != -1 || x.StartsWith("t"));
        }

        public int Solve(int part = 1)
            => Find3Groups();
    }
}
