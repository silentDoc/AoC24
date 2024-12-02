namespace AoC24.Day02
{
    internal class ReportChecker
    {
        List<List<int>> Reports = new();

        List<int> ParseLine(string line)
            => line.Split(' ').Select(int.Parse).ToList();

        public void ParseInput(List<string> input)
            => input.ForEach(x => Reports.Add(ParseLine(x)));

        bool IsSafe(List<int> report)
        {
            var difs = report.Skip(1).Zip(report, (x, y) => x - y);

            var sameSign = difs.All(x => x < 0) || difs.All(x => x > 0);
            var inDifRange = difs.Select(x => Math.Abs(x)).All(x => x < 4);
            
            return sameSign && inDifRange;
        }

        int FindSafeReports(int part)
            => Reports.Count(x => IsSafe(x));

        public int Solve(int part = 1)
            => FindSafeReports(part);
    }
}
