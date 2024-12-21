using AoC24.Common;
using System.Text;

namespace AoC24.Day21
{
    class KeyPad
    {
        Coord2D gap;
        Dictionary<char, Coord2D> Keys;
        List<(char Char, Coord2D Direction)> Directions = [('<', (-1, 0)), ('v', (0, 1)), ('^', (0, -1)), ('>', (1, 0))];

        public KeyPad(Dictionary<char, Coord2D> keyMap)
        {
            Keys = keyMap;
            gap = Keys[' '];
        }

        public string GetMoveSequence(string input)
        {
            StringBuilder sb = new();
            char from = 'A';
            foreach (var to in input)
            {
                sb.Append(MoveFromTo(from, to));
                from = to;
            }
            return sb.ToString();
        }

        public string MoveFromTo(char from, char to)
        {
            var sb = new StringBuilder();
            var toPos   = Keys[to];
            var fromPos = Keys[from];
            var dif = toPos - fromPos;
            var d = 0;

            while (dif != (0, 0))
            {
                // The trick here is that have the directions SORTED by distance to the A key
                // we find the first one that take us closer to our target
                var (dirChar, dir) = Directions[(d++ % Directions.Count)];
                var amount = dir.x == 0 ? dif.y / dir.y : dif.x / dir.x;
                
                if (amount <= 0)
                    continue;
                
                var dest = fromPos + dir * amount;
                if (dest == gap)
                    continue;
                
                fromPos = dest;
                dif -= dir * amount;
                sb.Append(new string(dirChar, amount));
            }
            sb.Append('A');
            return sb.ToString();
        }
    }

    internal class KeyPadPusher
    {
        // Gap = -10, A = 100
        Dictionary<char, Coord2D> NumPad = new() { { '7' , (0,0)} , { '8', (1, 0) }, { '9', (2, 0) } ,
                                                   { '4' , (0,1)} , { '5', (1, 1) }, { '6', (2, 1) } ,
                                                   { '1' , (0,2)} , { '2', (1, 2) }, { '3', (2, 2) } ,
                                                   { ' ' , (0,3)} , { '0', (1, 3) }, { 'A', (2, 3) } ,
                                                };

        Dictionary<char, Coord2D> DirPad = new() { { ' ' , (0,0)} , { '^', (1, 0) }, { 'A', (2, 0) } ,
                                                   { '<' , (0,1)} , { 'v', (1, 1) }, { '>', (2, 1) }    };
        List<string> codes = new();

        public void ParseInput(List<string> lines)
            => codes = lines;

        // Left here because this has been handy to find out why a test case was not working for me (379A)
        string ReverseEngineer(string sequence, Dictionary<char, Coord2D> keypad)
        {
            Coord2D pos = keypad['A'];
            StringBuilder sb = new();

            foreach (var c in sequence)
            {
                if (c == 'A')
                {
                    var val = keypad.Keys.First(x => keypad[x] == pos);
                    sb.Append(val);
                    continue;
                }

                pos += c switch
                {
                    '^' => (0, -1),
                    'v' => (0, 1),
                    '<' => (-1, 0),
                    '>' => (1, 0),
                    _ => throw new Exception("F")
                };
            }
            return sb.ToString();
        }

        long GetComplexity(string code)
        {
            KeyPad numPad = new(NumPad);
            KeyPad dirPad = new(DirPad);

            var seqNumPad = numPad.GetMoveSequence(code);
            var seqDirPad1 = dirPad.GetMoveSequence(seqNumPad);
            var seqDirPad2 = dirPad.GetMoveSequence(seqDirPad1);

            //var minLength = seqDirPad1.Length > seqDirPad2.Length ? seqDirPad2.Length : seqDirPad1.Length;
            var numCode = long.Parse(code.Substring(0, code.Length - 1));

            return ((long) seqDirPad2.Length) * numCode;
        }

        public long  Solve(int part = 1)
            =>codes.Sum(x => GetComplexity(x));

    }
}
