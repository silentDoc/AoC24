namespace AoC24.Day07
{
    class Calibration
    {
        public long TestValue;
        public List<long> Numbers;

        public Calibration(string inputLine)
        {
            var vals = inputLine.Replace(":", "").Split(' ').Select(long.Parse).ToList();
            TestValue = vals[0];
            Numbers = vals.Skip(1).ToList();
        }

        public int FindWays(int part = 1)
        {
            List<long> acums = [ Numbers[0] ];

            foreach (long num in Numbers.Skip(1))
            {
                List<long> newAcums = new();
                foreach (long inter in acums)
                {
                    long tmp;   // Avoiding calculating twice

                    if ((tmp = inter + num) <= TestValue)
                        newAcums.Add(tmp);
                    if ((tmp = inter * num) <= TestValue)
                        newAcums.Add(tmp);
                    if (part == 2 && (tmp = long.Parse($"{inter}{num}")) <= TestValue)
                        newAcums.Add(tmp);
                }
                acums = newAcums;
            }
            return acums.Count(x => x == TestValue);
        }
    }

    internal class RopeBridgeCalibrator
    {
        List<Calibration> calibrations = new();

        public void ParseInput(List<string> lines)
            => lines.ForEach(x => calibrations.Add(new Calibration(x)));

        public long Solve(int part = 1)
            => calibrations.Where(x => x.FindWays(part) > 0).Sum(x => x.TestValue);
    }
}
