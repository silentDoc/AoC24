using AoC24.Common;
using System.Text.RegularExpressions;

namespace AoC24.Day04
{
    internal class LetterSoup
    {
        Dictionary<Coord2D, char> soup = new();
        List<string> Input = [];
        int Rows = 0;
        int Cols = 0;

        void ParseLine((int index, string line) element)
        { 
            for(int i = 0; i<element.line.Length; i++) 
                soup[(i, element.index)] = element.line[i];
        }

        public void ParseInput(List<string> input)
        {
            input.Index().ToList().ForEach(ParseLine);
            Input = input;
            Rows = Input.Count();
            Cols = Input[0].Length;
        }

        int FindXmas()
        {
            var horizontal = Input.Sum(line => Regex.Matches(line, "XMAS").Count) + Input.Sum(line => Regex.Matches(line, "SAMX").Count);
            var verticalKeys = soup.Keys.Where(x => x.y <= Rows - 4);
            var verticalKeysBack = soup.Keys.Where(x => x.y >= 3);
            var diagonalKeys = soup.Keys.Where(k => k.y < Rows - 4 && k.x < Cols - 4);
            var diagonalKeysBack = soup.Keys.Where(k => k.y >= 3 && k.x >=3);
            var diagonalKeysRev = soup.Keys.Where(k => k.y < Rows - 4 && k.x >=3);
            var diagonalKeysRevBack = soup.Keys.Where(k => k.y >=3 && k.x < Cols-4);


            return horizontal +
                   verticalKeys.Count(k => soup[k] == 'X' && soup[k + (0, 1)] == 'M' && soup[k + (0, 2)] == 'A' && soup[k + (0, 3)] == 'S') +
                   verticalKeysBack.Count(k => soup[k] == 'S' && soup[k - (0, 1)] == 'A' && soup[k - (0, 2)] == 'M' && soup[k - (0, 3)] == 'X') +
                   diagonalKeys.Count(k => soup[k] == 'X' && soup[k + (1, 1)] == 'M' && soup[k + (2, 2)] == 'A' && soup[k + (3, 3)] == 'S') +
                   diagonalKeysBack.Count(k => soup[k] == 'S' && soup[k - (1, 1)] == 'A' && soup[k - (2, 2)] == 'M' && soup[k - (3, 3)] == 'X') +
                   diagonalKeysRev.Count(k => soup[k] == 'X' && soup[k + (-1, 1)] == 'M' && soup[k + (-2, 2)] == 'A' && soup[k + (-3, 3)] == 'S') +
                   diagonalKeysRevBack.Count(k => soup[k] == 'X' && soup[k + (1, -1)] == 'M' && soup[k + (2, -2)] == 'A' && soup[k + (3, -3)] == 'S');

        }

        public int Solve(int part = 1)
            => FindXmas();
    }
}
