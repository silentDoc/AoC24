using System.Text.RegularExpressions;

namespace AoC24.Day03
{
    internal class MemoryParser
    {
        List<string> lines = [];

        public void ParseInput(List<string> input)
            => lines = input;

        string Clean(string line)
        {
            char[] validChars = ['m', 'u', 'l', '(', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ')', ',', ' '];
            var invalidCharsInLine = line.Select(x => x).Distinct().Where(x => !validChars.Contains(x)).ToList();
            
            string result = line;
            foreach (var invalidChar in invalidCharsInLine)
                line = line.Replace(invalidChar.ToString(), "");

            return line;
        }

        int FindMulSum()
        {
            string pattern = @"mul\(([0-9]{1,3}),([0-9]{1,3})\)";
            Regex regex = new(pattern);
            int result = 0;

            var fullLine = string.Concat(lines);
            //var cLine = Clean(fullLine);
            
            foreach (Match match in regex.Matches(fullLine))
               result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            
            return result;
        }


        public int Solve(int part = 1)
            => FindMulSum();
    }
}
