namespace AoC24.Day01
{
    internal class LocationSolver
    {
        List<int> left = new();
        List<int> right = new();

        public void ParseInput(List<string> input)
        { 
            var values = input.Select(x => x.Split("   ").Select(int.Parse).ToList()).ToList();
            left = values.Select(x => x[0]).OrderBy(x => x).ToList();
            right = values.Select(x => x[1]).OrderBy(x => x).ToList();
        }

        int FindTotalDistance()
            => left.Zip(right, (x, y) => Math.Abs(y - x)).Sum();

        int SimilarityScore()
            => left.Distinct().Select(x => right.Count(el => el==x) * x).Sum();

        public int Solve(int part = 1)
            => part == 1? FindTotalDistance() : SimilarityScore();
    }
}
