using AoC24.Common;
using System.Security.Cryptography.X509Certificates;

namespace AoC24.Day05
{
    internal class PageOrderChecker
    {
        List<List<int>> rules = [];
        List<List<int>> updates = [];

        public void ParseInput(List<string> input)
        {
            var sections = ParseUtils.SplitBy(input, "");

            sections[0].ForEach(s => rules.Add(s.Split('|').Select(int.Parse).ToList()));
            sections[1].ForEach(s => updates.Add(s.Split(',').Select(int.Parse).ToList()));
        }

        bool IsCorrect(List<int> update)
        { 
            var rulesToApply = rules.Where(r => update.Contains(r[0]) && update.Contains(r[1]))
                                                      .Select(r => update.IndexOf(r[0]) < update.IndexOf(r[1]))
                                                      .ToList();
            return rulesToApply.All(x => x);
        }

        int FindCorrectUpdates()
        {
            var correctUpdates = updates.Where(u => IsCorrect(u)).ToList();
            var retVal = 0;
            foreach (var update in correctUpdates)
                retVal += update[update.Count() / 2];
            
            return retVal;
        }

        public int Solve(int part = 1)
            => FindCorrectUpdates();
    }
}
