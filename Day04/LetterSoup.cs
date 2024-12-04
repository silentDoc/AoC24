using AoC24.Common;
using System.Text.RegularExpressions;

namespace AoC24.Day04
{
    internal class LetterSoup
    {
        Dictionary<Coord2D, char> soup = new();
        List<string> Input = [];
        int MaxY = 0;
        int MaxX = 0;

        void ParseLine((int index, string line) element)
        { 
            for(int i = 0; i<element.line.Length; i++) 
                soup[(i, element.index)] = element.line[i];
        }

        public void ParseInput(List<string> input)
        {
            input.Index().ToList().ForEach(ParseLine);
            Input = input;
            MaxY = soup.Keys.Max(k => k.y);
            MaxX = soup.Keys.Max(k => k.x);
        }

        int FindXmas()
        {
            var verticalKeys = soup.Keys.Where(x => x.y <= MaxY - 3);
            var diagonalKeys_LR = soup.Keys.Where(k => k.y <= MaxY - 3 && k.x <= MaxX - 3);
            var diagonalKeys_RL = soup.Keys.Where(k => k.y >= 3 && k.x <= MaxX - 3);

            var horizontal = Input.Sum(line => Regex.Matches(line, "XMAS").Count) + Input.Sum(line => Regex.Matches(line, "SAMX").Count);
            var vertical = verticalKeys.Select(k => new string([soup[k], soup[k + (0, 1)], soup[k + (0, 2)], soup[k + (0, 3)]])).Count(s => s == "XMAS" || s == "SAMX");
            var diag_LR = diagonalKeys_LR.Select(k => new string([soup[k], soup[k + (1, 1)], soup[k + (2, 2)], soup[k + (3, 3)]])).Count(s => s == "XMAS" || s == "SAMX");
            var diag_RL = diagonalKeys_RL.Select(k => new string([soup[k], soup[k + (1, -1)], soup[k + (2, -2)], soup[k + (3, -3)]])).Count(s => s == "XMAS" || s == "SAMX");

            return horizontal + vertical + diag_LR + diag_RL;
        }

        int FinsStars()
        {
            var validKeys = soup.Keys.Where(k => k.x>0 && k.x<MaxX && k.y<MaxY && k.y>0).ToList();

            var stars = validKeys.Where(k => soup[k] == 'A' && ( soup[k + (-1, -1)] == soup[k + (-1, 1)] || soup[k + (-1, -1)] == soup[k + (1, -1)] ))
                                 .Select(k => new List<char>([soup[k + (-1, -1)], soup[k + (1, 1)], soup[k + (-1, 1)], soup[k + (1, -1)]])).ToList();

            return stars.Count(l => l.Count(c => c == 'M') == 2 && l.Count(c => c == 'S') == 2);
        }

        public int Solve(int part = 1)
            => part == 1? FindXmas() : FinsStars();
    }
}
