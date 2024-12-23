namespace AoC24.Day23
{
    internal class LanParty
    {
        Dictionary<string, HashSet<string>> conns = new();

        void ParseLine(string line)
        {
            var pcs = line.Split("-");
            if (!conns.ContainsKey(pcs[0]))
                conns[pcs[0]] = new();
            if (!conns.ContainsKey(pcs[1]))
                conns[pcs[1]] = new();
            conns[pcs[0]].Add(pcs[1]);
            conns[pcs[1]].Add(pcs[0]);
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

        string FindLargestGroup()
        {
            HashSet<string> groups = new();

            foreach (var computer in conns.Keys)
            {
                var connectedComputers = conns[computer];
                foreach (var neighbor in connectedComputers)
                {
                    var commonPCs = connectedComputers.Intersect(conns[neighbor]).ToHashSet();

                    if (commonPCs.Count <= 1)
                        continue;

                    // We know that computer and neighbor are connected to each one of the pcs of the intersection
                    // now we have to keep only the pcs of the intersection that are connected to every other pc of the 
                    // intersection
                    HashSet<string> lanGroup = new();

                    foreach (var pc in commonPCs)
                    {
                        var restOfIntersect = commonPCs.Where(x => x != pc).ToHashSet();
                        if(restOfIntersect.All(x => conns[pc].Contains(x)))
                            lanGroup.Add(pc);
                    }

                    List<string> largeGroup = [computer, neighbor, ..lanGroup.ToList()];
                    var str = string.Join(',', largeGroup.OrderBy(y => y));
                    groups.Add(str);
                }
            }

            var maxLength = groups.Max(x => x.Length);
            return groups.First(x => x.Length == maxLength);
        }

        public string Solve(int part = 1)
            => part == 1 ? Find3Groups().ToString() : FindLargestGroup();
    }
}
