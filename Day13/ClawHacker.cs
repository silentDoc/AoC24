using AoC24.Common;

namespace AoC24.Day13
{
    record ClawMachine(Coord2DL buttonA, Coord2DL buttonB, Coord2DL prize);

    internal class ClawHacker
    {
        List<ClawMachine> machines = [];

        ClawMachine ParseMachine(List<string> section)
        {
            var butAStr = section[0].Replace("Button A: X", "").Replace("Y", "");
            var butBStr = section[1].Replace("Button B: X", "").Replace("Y", "");
            var prizeStr = section[2].Replace("Prize: X=", "").Replace("Y=", "");

            var vA = butAStr.Split(",").Select(long.Parse).ToList();
            var vB = butBStr.Split(",").Select(long.Parse).ToList();
            var vP = prizeStr.Split(",").Select(long.Parse).ToList();

            return new ClawMachine(new Coord2DL(vA[0], vA[1]), new Coord2DL(vB[0], vB[1]), new Coord2DL(vP[0], vP[1]));
        }

        public void ParseInput(List<string> input)
        {
            var sections = ParseUtils.SplitBy(input, "");
            sections.ForEach(x => machines.Add(ParseMachine(x)));
        }

        long SolveMachine(ClawMachine m)
        {
            // See attached linearAlgebra.jpg to see where this comes from
            double timesB = (double)(m.prize.y * m.buttonA.x - m.buttonA.y * m.prize.x) / (double)(m.buttonA.x * m.buttonB.y - m.buttonA.y * m.buttonB.x);
            double timesA = (double)(m.prize.x - timesB * m.buttonB.x) / (double)m.buttonA.x;

            if (timesB != Math.Floor(timesB))
                return 0;
            if (timesA != Math.Floor(timesA))
                return 0;
            if (timesA < 0 || timesB < 0)
                return 0;

            return (long) (timesA * 3 + timesB);
        }
     
        long FindHowManyPrizes()
            => machines.Sum(x => SolveMachine(x));

        long FindHowManyPrizesFarAway()
        {
            List<ClawMachine> farAwayMachines = [];
            machines.ForEach(x => farAwayMachines.Add(new ClawMachine(x.buttonA, x.buttonB, new Coord2DL(x.prize.x + 10000000000000, x.prize.y + 10000000000000))));
            return farAwayMachines.Sum(x => SolveMachine(x));
        }

        public long Solve(int part = 1)
            => part == 1 ? FindHowManyPrizes() : FindHowManyPrizesFarAway();
    }
}
