namespace AoC24.Day07
{
    class Calibration
    {
        public long TestValue;
        public List<long> Numbers;
        List<List<char>> Operators = new();

        public Calibration(string inputLine)
        { 
            var vals = inputLine.Replace(":", "").Split(' ').Select(long.Parse).ToList();
            TestValue = vals[0];
            Numbers = vals.Skip(1).ToList();
        }

        public bool Eval(List<char> operators)
        {
            var acum = Numbers[0];

            for (int i = 1; i < Numbers.Count; i++)
                acum = operators[i-1] == '+' ? acum + Numbers[i] : acum * Numbers[i];

            return acum == TestValue;
        }

        public int FindWaysToSolve()
        {
            // Calc all the possible combinations

            List<List<char>> ops = new();
            for (int i = 0; i < Numbers.Count-1; i++)
            {
                if (ops.Count == 0)
                {
                    ops.Add(['+']);
                    ops.Add(['*']);
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

        long FindSum()
        { 
            var possibleCalibrations = calibrations.Where(x => x.FindWaysToSolve() > 0).ToList();
            return possibleCalibrations.Sum(x => x.TestValue);
        }

        public long Solve(int part = 1)
            => FindSum();
    }
}
