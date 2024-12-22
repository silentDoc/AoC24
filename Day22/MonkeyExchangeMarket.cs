using AoC24.Common;

namespace AoC24.Day22
{
    internal class MonkeyExchangeMarket
    {
        List<long> startValues = new();
        Dictionary<int, List<long>> allSecrets = new();

        public void ParseInput(List<string> input) 
            => startValues = input.Select(long.Parse).ToList();

        long FindSecret(long secret, int iterations, int buyer = -1)
        {
            long res = secret;
            for (int i = 0; i < iterations; i++)
            {
                res = ((res << 6 ) ^ res) % 16777216;
                res = ((res >> 5 ) ^ res) % 16777216;
                res = ((res << 11) ^ res) % 16777216;

                if (buyer != -1)
                    allSecrets[buyer].Add(res);
            }
            return res;
        }

        long GetMostBananas()
        {
            Dictionary<int, List<int>> numBananas = new();
            int iterations = 1999;

            for (int i = 0; i < startValues.Count; i++)
            {
                allSecrets[i] = new();
                numBananas[i] = new();
            }

            for (int i = 0; i < startValues.Count; i++)
            {
                allSecrets[i].Add(startValues[i]);
                _ = FindSecret(startValues[i], iterations, i);    // Populates allSecrets
            }

            for (int i = 0; i < allSecrets.Count; i++)            // Populates the number of bananas sold
                foreach (var num in allSecrets[i])
                    numBananas[i].Add((int)num % 10);

            // Now the hard part

            // Build all the possible sequences that trigger the monkeys to buy
            List<Dictionary<string, int>> allBuyerSeqs = new();
            for (int i = 0; i < numBananas.Count; i++)            
            {
                Dictionary<string, int> buyerSeqs = new();
                var bananasSold = numBananas[i];
                foreach (var window in bananasSold.Windowed(5))
                {
                    var difs = window.Skip(1).Zip(window, (a, b) => a - b).ToList();
                    var key = string.Concat(difs);

                    if (!buyerSeqs.ContainsKey(key))
                        buyerSeqs[key] = window.Last();
                }
                allBuyerSeqs.Add(buyerSeqs);
            }

            // At this point, for each buyer we have all the possible price dif sequences that happen in the first 2000 secrets
            // along with the bananas that will be sold. We have to find the sequence that gets us the most now
            var allKeysDistinct = allBuyerSeqs.SelectMany(x => x.Keys).ToHashSet();
            Dictionary<string, int> totalSold = new Dictionary<string, int>();

            foreach (var key in allKeysDistinct)
                totalSold[key] = allBuyerSeqs.Sum(x => x.GetValueOrDefault(key,0));

            var foundKey = totalSold.Keys.First(x => totalSold[x] == totalSold.Values.Max());
            Console.WriteLine(foundKey);

            return totalSold.Values.Max();
        }

        public long Solve(int part = 1)
            => part == 1 ? startValues.Sum(x => FindSecret(x, 2000)) : GetMostBananas();
    }
}
