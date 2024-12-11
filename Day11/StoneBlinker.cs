namespace AoC24.Day11
{
    internal class StoneBlinker
    {
        List<long> stones = [];

        public void ParseInput(List<string> input)
            => stones = input[0].Split(" ").Select(long.Parse).ToList();

        long[] Blink(long num)
        {
            if (num == 0)
                return [1, -1];
           
            var numDigits = Math.Floor(Math.Log10(num) + 1);

            if (numDigits % 2 == 0)
            {
                var decider = (long)Math.Pow(10, numDigits / 2);
                long left = num / decider;
                long right = num % decider;
                return [left, right];
            }
            else
                return [num * 2024, -1];
         }


        long FindStonesFast(int numBlinks)
        {
            Dictionary<long, long[]> descendants = new();
            Dictionary<long, long> stoneCount = new();

            foreach(var stone in stones)
            {
                descendants[stone] = Blink(stone);
                stoneCount[stone] = 1;
            }

            for (int i = 0; i < numBlinks; i++)
            {
                Dictionary<long, long> newStoneCount = new();
                foreach (var stone in stoneCount.Keys)
                {
                    if (!descendants.ContainsKey(stone))
                        descendants[stone] = Blink(stone);

                    var desc = descendants[stone];
                    var first = desc[0];
                    var second = desc[1];

                    newStoneCount[first] = newStoneCount.ContainsKey(first) ? newStoneCount[first] + stoneCount[stone]
                                                                            : stoneCount[stone];

                    if (second!=-1)
                        newStoneCount[second] = newStoneCount.ContainsKey(second) ? newStoneCount[second] + stoneCount[stone]
                                                                                  : stoneCount[stone];
                }
                stoneCount = newStoneCount;
            }

            return stoneCount.Values.Sum();
        }

        public long Solve(int part = 1)
            => part == 1 ? FindStonesFast(25) : FindStonesFast(75);
    }
}
