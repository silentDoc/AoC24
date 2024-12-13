using AoC24.Common;

namespace AoC24.Day13
{

    record ClawMachine(Coord2D buttonA, Coord2D buttonB, Coord2D prize);

    internal class ClawHacker
    {
        List<ClawMachine> machines = [];

        ClawMachine ParseMachine(List<string> section)
        {
            var butAStr = section[0].Replace("Button A: X", "").Replace("Y", "");
            var butBStr = section[1].Replace("Button B: X", "").Replace("Y", "");
            var prizeStr = section[2].Replace("Prize: X=", "").Replace("Y=", "");

            var vA = butAStr.Split(",").Select(int.Parse).ToList();
            var vB = butBStr.Split(",").Select(int.Parse).ToList();
            var vP = prizeStr.Split(",").Select(int.Parse).ToList();

            return new ClawMachine(new Coord2D(vA[0], vA[1]), new Coord2D(vB[0], vB[1]), new Coord2D(vP[0], vP[1]));
        }

        public void ParseInput(List<string> input)
        {
            var sections = ParseUtils.SplitBy(input, "");
            sections.ForEach(x => machines.Add(ParseMachine(x)));
        }

        int SolveEqSystem(double[] xs, double[] ys, double[] eqs)
        {
            double[,] eliminator = new double[2,2];
            int reward = 0;
            
            eliminator[0, 0] = ys[1] * xs[0];
            eliminator[0, 1] = ys[1] * eqs[0];
            eliminator[1, 0] = ys[0] * xs[1];
            eliminator[1, 1] = ys[0] * eqs[1];

            try
            {
                var timesA = (eliminator[0, 1] - eliminator[1, 1]) / (eliminator[0, 0] - eliminator[1, 0]);
                var timesB = (eqs[0] - xs[0] * timesA) / ys[0];

                var test = timesA * xs[0] + timesB * ys[0] == eqs[0];
                var test2 = timesA * xs[1] + timesB * ys[1] == eqs[1];

                if (timesA < 0 || timesB < 0)
                    return 0;

                if (Math.Floor(timesA) != timesA)
                    return 0;

                if (Math.Floor(timesB) != timesB)
                    return 0;

                reward = (int) Math.Round(timesA * 3 + timesB * 3);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return reward;
        }


        int SolveMachine(ClawMachine machine)
        {
            Coord2D currentDistance = machine.prize;
            var timesA = 0;

            while (currentDistance.x % machine.buttonB.x != 0 && currentDistance.y / machine.buttonB.y != currentDistance.x / machine.buttonB.x)
            {
                if (currentDistance.x < 0 || currentDistance.y < 0)
                    return 0;

                currentDistance -= machine.buttonA;
                timesA++;
            }
            var timesB = currentDistance.x / machine.buttonB.x;

            return timesA * 80 + timesB * 40;
        }

        int SolveMachine2(ClawMachine m)
        {
            double timesB = (double)(m.prize.y * m.buttonA.x - m.buttonA.y * m.prize.x) / (double)(m.buttonA.x * m.buttonB.y - m.buttonA.y * m.buttonB.x);
            double timesA = (double)(m.prize.x - timesB * m.buttonB.x) / (double)m.buttonA.x;

            if (timesB != Math.Floor(timesB))
                return 0;
            if (timesA != Math.Floor(timesA))
                return 0;
            if (timesA < 0 || timesB < 0)
                return 0;

            return (int) (timesA * 3 + timesB);
        }

        int Prize(ClawMachine machine)
            => SolveEqSystem([machine.buttonA.x, machine.buttonA.y], [machine.buttonB.x, machine.buttonB.y], [machine.prize.x, machine.prize.y]);

        int FindHowManyPrizes()
            => machines.Sum(x => SolveMachine2(x));

        public int Solve(int part = 1)
            => FindHowManyPrizes();
    }
}
