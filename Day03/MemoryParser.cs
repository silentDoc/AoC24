using System.Text.RegularExpressions;

namespace AoC24.Day03
{
    class RegexElement
    {
        public string Type = "";
        public int Position = 0;
        public GroupCollection? Group = null;

        public RegexElement(string type, int pos, GroupCollection group)
        {
            Type = type;
            Position = pos;
            Group = group;
        }
    }

    internal class MemoryParser
    {
        List<string> lines = [];
        string patternMul = @"mul\(([0-9]{1,3}),([0-9]{1,3})\)";
        string patternDo = @"do\(\)";
        string patternDont = @"don't\(\)";


        public void ParseInput(List<string> input)
            => lines = input;

        int FindMulSum()
        {
            Regex regex = new(patternMul);
            int result = 0;

            var fullLine = string.Concat(lines);
            foreach (Match match in regex.Matches(fullLine))
               result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            
            return result;
        }

        int FindMulSumEnabling()
        {
            Regex regexMul = new(patternMul);
            Regex regexDo = new(patternDo);
            Regex regexDont = new(patternDont);

            int result = 0;
            var fullLine = string.Concat(lines);

            var mulPositions = regexMul.Matches(fullLine).Select(x => new RegexElement("Mul", x.Index, x.Groups)).ToList();
            var doPositions = regexDo.Matches(fullLine).Select(x => new RegexElement("Do", x.Index, x.Groups)).ToList();
            var dontPositions = regexDont.Matches(fullLine).Select(x => new RegexElement("Dont", x.Index, x.Groups)).ToList();

            List<RegexElement> fullList = [..mulPositions,..doPositions,..dontPositions];

            var enabled = true;
            foreach (RegexElement elem in fullList.OrderBy(x => x.Position))
            {
                if (elem.Type == "Do")
                    enabled = true;
                if (elem.Type == "Dont")
                    enabled = false;
                if (elem.Type == "Mul" && enabled)
                    result += int.Parse(elem.Group[1].Value) * int.Parse(elem.Group[2].Value);
            }
            return result;
        }

        public int Solve(int part = 1)
            => part == 1 ? FindMulSum() : FindMulSumEnabling();
    }
}
