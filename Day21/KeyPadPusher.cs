using AoC24.Common;
using System.Text;

namespace AoC24.Day21
{
    class KeyPad
    {
        Coord2D gap;
        Dictionary<char, Coord2D> Keys;
        Dictionary<(char, char), string> MemoizeMoves = new();
        // The order of this list is the key to the problem today - Distance from each key to the A key in the direction keypad
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
            if (MemoizeMoves.ContainsKey((from, to)))
                return MemoizeMoves[(from, to)];

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
                
                // We divide by one, but we keep the sign
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
            MemoizeMoves[(from, to)] = sb.ToString();
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
        KeyPad globalDirPad;

        Dictionary<(string, int), long>  MemoizeSeqs  = new();
        
        List<string> codes = new();

        public void ParseInput(List<string> lines)
            => codes = lines;

        // Our recursive method with memoization
        long FindSeqLength(string code, int level)  
        {
            if (level == 0)
                return code.Length;

            var keyMemo = (code, level);
            if (MemoizeSeqs.ContainsKey(keyMemo))
                return MemoizeSeqs[keyMemo];

            // If we do not have it memoized, we have to work it out
            var nextLevel = globalDirPad.GetMoveSequence(code);
            var nextSubSeqs = nextLevel.Split('A').Select(x => x + "A").ToList();
            nextSubSeqs.RemoveAt(nextSubSeqs.Count() - 1);

            MemoizeSeqs[keyMemo] = nextSubSeqs.Sum(x => FindSeqLength(x, level - 1));
            return MemoizeSeqs[keyMemo];
        }

        long GetComplexity(string code, int levels)
        {
            KeyPad numPad = new(NumPad);
            KeyPad dirPad = new(DirPad);
            globalDirPad = new KeyPad(DirPad);

            var seq = numPad.GetMoveSequence(code);
            
            // The "A" button has the property of being the END position for each sequence at every level
            // For Part 2, we will break the code into splits of subcodes ending with A and memoize all the levels
            long length = FindSeqLength(seq, 25);
            var  numCode = long.Parse(code.Substring(0, code.Length - 1));

            return numCode * length;
        }

        public long Solve(int part = 1)
            => part == 1 ? codes.Sum(x => GetComplexity(x, 2)) : codes.Sum(x => GetComplexity(x, 25));
    }
}
