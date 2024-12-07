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

        public bool Eval(List<char> operators, int part = 1)
        {
            var acum = Numbers[0];
            for (int i = 1; i < Numbers.Count; i++)
                acum = operators[i - 1] switch
                {
                    '+' => acum + Numbers[i],
                    '*' => acum * Numbers[i],
                    '|' => long.Parse(acum.ToString() + Numbers[i].ToString()),
                    _ => throw new Exception("Unknown operator " + operators[i - 1].ToString())
                };
            return acum == TestValue;
        }

        public int FindWaysToSolve(int part =1)
        {
            // Calc all the possible combinations
            List<List<char>> ops = new();
            for (int i = 0; i < Numbers.Count-1; i++)
            {
                if (ops.Count == 0)
                {
                    ops.Add(['+']);
                    ops.Add(['*']);
                    if(part == 2)
                        ops.Add(['|']);

                    continue;
                }

                List<List<char>> newList = new();
                foreach (var l in ops)
                { 
                    var tmp = l.ToList();
                    var tmp2 = l.ToList();

                    tmp.Add('+');
                    tmp2.Add('*');

                    newList.Add(tmp);
                    newList.Add(tmp2);

                    if (part == 2)
                    {
                        var tmp3 = l.ToList();
                        tmp3.Add('|');
                        newList.Add(tmp3);
                    }

                    ops = newList;
                }
            }

            return ops.Count(x => Eval(x));
        }
    }

    internal class RopeBridgeCalibrator
    {
        List<Calibration> calibrations = new();
        public void ParseInput(List<string> lines)
            => lines.ForEach(x => calibrations.Add(new Calibration(x)));

        long FindSum(int part)
            => calibrations.Where(x => x.FindWaysToSolve(part) > 0).Sum(x => x.TestValue);

        public long Solve(int part = 1)
            => FindSum(part);
    }
}
