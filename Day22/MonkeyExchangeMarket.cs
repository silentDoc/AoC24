namespace AoC24.Day22
{
    internal class MonkeyExchangeMarket
    {
        List<long> startValues = new();
        public void ParseInput(List<string> input) 
            => startValues = input.Select(long.Parse).ToList();

        long FindSecret(long secret, int iterations)
        {
            long res = secret;
            for (int i = 0; i < iterations; i++)
            {
                res = ((res << 6 ) ^ res) % 16777216;
                res = ((res >> 5 ) ^ res) % 16777216;
                res = ((res << 11) ^ res) % 16777216;
            }
            return res;
        }

        public long Solve(int part = 1)
            => startValues.Sum(x => FindSecret(x, 2000));
    }
}
