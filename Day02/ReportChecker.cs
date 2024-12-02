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

        bool IsSafeTolerate(List<int> report)
        {
            // Bruteforce

            if (IsSafe(report))
                return true;

            for (int i = 0; i < report.Count(); i++)
            {
                var reportCopy = new List<int>();
                reportCopy.AddRange(report);

                reportCopy.RemoveAt(i);

                if (IsSafe(reportCopy))
                    return true;
            }
            return false;
        }

        int FindSafeReports(int part)
            => part == 1 ? Reports.Count(x => IsSafe(x)) : Reports.Count(x => IsSafeTolerate(x));


        public int Solve(int part = 1)
            => FindSafeReports(part);
    }
}
