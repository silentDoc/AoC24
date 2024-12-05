using AoC24.Common;

namespace AoC24.Day05
{
    internal class PageOrderChecker
    {
        List<List<int>> rules = [];
        List<List<int>> updates = [];
        Comparer<int> customComparer;

        public void ParseInput(List<string> input)
        {
            var sections = ParseUtils.SplitBy(input, "");

            sections[0].ForEach(s => rules.Add(s.Split('|').Select(int.Parse).ToList()));
            sections[1].ForEach(s => updates.Add(s.Split(',').Select(int.Parse).ToList()));
            customComparer = CreatePageComparer();
        }

        Comparer<int> CreatePageComparer()
            => Comparer<int>.Create((a, b) => rules.Any(rule => rule[0] == a && rule[1] == b) ? -1
                                              : rules.Any(rule => rule[0] == b && rule[1] == a) ? 1
                                              : 0);

        bool IsCorrect(List<int> update)
            => rules.Where(r => update.Contains(r[0]) && update.Contains(r[1]))
                    .Select(r => update.IndexOf(r[0]) < update.IndexOf(r[1]))
                    .All(x => x);

        int FindCorrectUpdates()
        {
            var correctUpdates = updates.Where(u => IsCorrect(u)).ToList();
            return correctUpdates.Sum(x => x[x.Count() / 2]);
        }

        int ReorderIncorrect()
        {
            var incorrectUpdates = updates.Where(u => !IsCorrect(u)).ToList();
            var fixedUpdates = incorrectUpdates.Select(x => x.OrderBy(y => y, customComparer).ToList());
            return fixedUpdates.Sum(x => x[x.Count() / 2]);
        }

        public int Solve(int part = 1)
            => part == 1 ? FindCorrectUpdates() : ReorderIncorrect();
    }
}
