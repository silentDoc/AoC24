namespace AoC24.Day19
{
    internal class TowelDesigner
    {
        List<string> onsens = [];
        List<string> patterns = [];
        Dictionary<string, long> memoize = new() { [""] = 1 };

        public  void ParseInput(List<string> input)
        {
            patterns = input[0].Split(", ").ToList();
            onsens = input[2..];
        }

        long FindWay(string remainingOnsen)
        {
            if (memoize.ContainsKey(remainingOnsen))
                return memoize[remainingOnsen];

            memoize[remainingOnsen] = patterns.Where(x => remainingOnsen.StartsWith(x))
                                              .Sum(y => FindWay(remainingOnsen.Substring(y.Length)));
            return memoize[remainingOnsen];
        }

        public int Solve(int part = 1)
            => onsens.Count(x => FindWay(x)>0);
    }
}
