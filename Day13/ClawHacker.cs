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

        int SolveMachine(ClawMachine m)
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
     
        int FindHowManyPrizes()
            => machines.Sum(x => SolveMachine(x));

        public int Solve(int part = 1)
            => FindHowManyPrizes();
    }
}
