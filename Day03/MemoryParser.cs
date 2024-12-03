using System.Text.RegularExpressions;

namespace AoC24.Day03
{
    record RegexElement(string Type, int Position, int MulValue);

    internal class MemoryParser
    {
        string Input = "";
        Regex regexMul = new(@"mul\(([0-9]{1,3}),([0-9]{1,3})\)");
        Regex regexDo = new(@"do\(\)");
        Regex regexDont = new(@"don't\(\)");

        public void ParseInput(List<string> input)
            => Input = string.Concat(input);

        int GetMul(GroupCollection group)
            => int.Parse(group[1].Value) * int.Parse(group[2].Value);

        int FindMulSum()
            => regexMul.Matches(Input).Select(m => GetMul(m.Groups)).Sum();

        int FindMulSumEnabling()
        {
            int result = 0;
            var mulPositions = regexMul.Matches(Input).Select(x => new RegexElement("Mul", x.Index, GetMul(x.Groups))).ToList();
            var doPositions = regexDo.Matches(Input).Select(x => new RegexElement("Do", x.Index, 0)).ToList();
            var dontPositions = regexDont.Matches(Input).Select(x => new RegexElement("Dont", x.Index, 0)).ToList();

            List<RegexElement> fullList = [..mulPositions,..doPositions,..dontPositions];

            var enabled = true;
            foreach (RegexElement elem in fullList.OrderBy(x => x.Position))
            {
                if (elem.Type == "Do")
                    enabled = true;
                if (elem.Type == "Dont")
                    enabled = false;
                if (elem.Type == "Mul" && enabled)
                    result += elem.MulValue;
            }
            return result;
        }

        public int Solve(int part = 1)
            => part == 1 ? FindMulSum() : FindMulSumEnabling();
    }
}
