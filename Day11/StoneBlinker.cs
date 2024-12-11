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

        long FindStonesFast(int numBlinks)
        {
            Stone first = new(stones[0]);
            Stone last = first;
            Stone current = first;
            
            for (int i = 1; i < stones.Count; i++)
            {
                current = new(stones[i]);
                last.next = current;
                last = current;
            }

            for (int i = 0; i < numBlinks; i++)
            {
                Console.WriteLine(i.ToString());
                current = first;

                while(current!=null)
                {

                    if (current.value == 0)
                    {
                        current.value = 1;
                        current = current.next;
                        continue;
                    }
                    
                    var numStr = current.value.ToString();
                    var numDigits = numStr.Length;

                    if (numDigits % 2 == 0)
                    {
                        var left = numStr.Substring(0, numDigits / 2);
                        var right = numStr.Substring(numDigits / 2);

                        current.value  = long.Parse(left);
                        var StoneRight = new Stone(long.Parse(right));
                        
                        StoneRight.next = current.next;
                        current.next = StoneRight;
                        current = StoneRight.next;
                    }
                    else
                    {
                        current.value *= 2024;
                        current = current.next;
                    }
                }
            }

            current = first;
            long total = 0;
            while (current != null)
            {
                total++;
                current = current.next;
            }

            return total;
        }


        int FindStones(int numBlinks)
        {
            for (int i = 0; i < numBlinks; i++)
            {
                Console.WriteLine(i.ToString() + " - " + stones.Count.ToString());
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
            => part == 1 ? FindStones(25) : FindStonesFast(75);
    }
}
