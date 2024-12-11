namespace AoC24.Day11
{
    internal class StoneBlinker
    {
        List<long> stones = [];

        public void ParseInput(List<string> input)
            => stones = input[0].Split(" ").Select(long.Parse).ToList();

        void Display(List<long> stones)
            => Console.Write(string.Join(" ", stones) + "\n");

        int FindStones(int numBlinks)
        {
            for (int i = 0; i < numBlinks; i++)
            {
                Console.WriteLine(stones.Count.ToString());
                List<long> newList = [];
                foreach (var stone in stones)
                {

                    if (stone == 0)
                    {
                        newList.Add(1);
                        continue;
                    }
                    var numStr = stone.ToString();
                    var numDigits = numStr.Length;

                    if (numDigits % 2 == 0)
                    { 
                        var left = numStr.Substring(0,numDigits/2);
                        var right = numStr.Substring(numDigits / 2);
                        newList.Add(long.Parse(left));
                        newList.Add(long.Parse(right));
                    }
                    else
                        newList.Add(stone * 2024);
                }
                stones = newList;
                
            }
            
            return stones.Count;
        }

        public long Solve(int part = 1)
            => FindStones(25);
    }
}
