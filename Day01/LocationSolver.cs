namespace AoC24.Day01
{
    internal class LocationSolver
    {
        List<int> Locations0 = new();
        List<int> Locations1 = new();

        public void ParseInput(List<string> input)
        { 
            var values = input.Select(x => x.Split("   ").Select(int.Parse).ToList()).ToList();
            Locations0 = values.Select(x => x[0]).OrderBy(x => x).ToList();
            Locations1 = values.Select(x => x[1]).OrderBy(x => x).ToList();
        }

        int FindTotalDistance()
            => Locations0.Zip(Locations1, (x, y) => Math.Abs(y - x)).Sum();

        public int Solve(int part = 1)
            => FindTotalDistance();
    }
}
