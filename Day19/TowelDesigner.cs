namespace AoC24.Day19
{
    internal class TowelDesigner
    {
        List<string> designs = [];
        List<string> patterns = [];
        Dictionary<string, long> memoize = new() { [""] = 1 };

        public  void ParseInput(List<string> input)
        {
            patterns = input[0].Split(", ").ToList();
            designs = input[2..];
        }

        long FindWay(string remainingDesign)
        {
            if (memoize.ContainsKey(remainingDesign))
                return memoize[remainingDesign];

            memoize[remainingDesign] = patterns.Where(x => remainingDesign.StartsWith(x))
                                              .Sum(y => FindWay(remainingDesign.Substring(y.Length)));
            return memoize[remainingDesign];
        }

        public long Solve(int part = 1)
            => part == 1 ? designs.Count(x => FindWay(x)>0) : designs.Sum(x => FindWay(x));
    }
}
