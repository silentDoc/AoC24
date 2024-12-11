namespace AoC24.Day11
{
    class Stone
    {
        public long value;
        public Stone? next;

        public Stone(long val)
        {
            value = val;
            next = null;
        }

    }



    internal class StoneBlinker
    {
        List<long> stones = [];

        public void ParseInput(List<string> input)
            => stones = input[0].Split(" ").Select(long.Parse).ToList();

        void Display(List<long> stones)
            => Console.Write(string.Join(" ", stones) + "\n");

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
            // LEts try some memoization

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


        int FindStones(int numBlinks)
        {
            Dictionary<long, (long, long, long)> memo = new();
            

            HashSet<long> test = new();
           
            for (int i = 0; i < numBlinks; i++)
            {
                Console.WriteLine(i.ToString() + " - " + stones.Count.ToString() + " - " + test.Count.ToString());
                List<long> newList = [];
                foreach (var stone in stones)
                {
                    if (memo.ContainsKey(stone))
                    {
                        newList.Add(memo[stone].Item1);
                        if (memo[stone].Item2 != -1)
                            newList.Add(memo[stone].Item2);

                        continue;
                    }

                    if (stone == 0)
                    {
                        newList.Add(1);
                        memo[0] = (1, -1, 1);
                        continue;
                    }
                    var numStr = stone.ToString();
                    var numDigits = numStr.Length;

                    if (numDigits % 2 == 0)
                    {
                        var left = long.Parse(numStr.Substring(0, numDigits / 2));
                        var right = long.Parse(numStr.Substring(numDigits / 2));
                        newList.Add(left);
                        newList.Add(right);
                        memo[stone] = (left, right, 2);
                    }
                    else
                    {
                        newList.Add(stone * 2024);
                        memo[stone] = (stone*2024, -1, 1);
                    }
                }
                stones = newList;
                
            }
            
            return stones.Count;
        }

        public long Solve(int part = 1)
            => part == 1 ? FindStones(25) : FindStonesFast(75);
    }
}
