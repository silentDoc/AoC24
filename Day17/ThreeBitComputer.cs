using AoC24.Common;
using AoC24.Day04;
using System;
using System.Text;

namespace AoC24.Day17
{
    internal class ThreeBitComputer
    {
        const int Ins_Adv = 0;
        const int Ins_Bxl = 1;
        const int Ins_Bst = 2;
        const int Ins_Jnz = 3;
        const int Ins_Bxc = 4;
        const int Ins_Out = 5;
        const int Ins_Bdv = 6;
        const int Ins_Cdv = 7;

        Dictionary<char, long> reg = new();
        List<int> sourceCode = new();
        List<long> outputBuffer = new();

        public void ParseInput(List<string> lines)
        {
            reg['A'] = int.Parse(lines[0].Replace("Register A: ", ""));
            reg['B'] = int.Parse(lines[1].Replace("Register B: ", ""));
            reg['C'] = int.Parse(lines[2].Replace("Register C: ", ""));
            sourceCode = lines[4].Replace("Program: ", "").Split(',').Select(int.Parse).ToList();
        }

        long ComboOperand(int operand)
            => operand switch
            {
                0 => operand,
                1 => operand,
                2 => operand,
                3 => operand,
                4 => reg['A'],
                5 => reg['B'],
                6 => reg['C'],
                _ => throw new Exception("Invalid operand")
            };
        

        int ExAdv(int insPtr)
        {
            long operand = ComboOperand(sourceCode[insPtr + 1]);
            long num = reg['A'];
            long den = (int) Math.Pow(2,operand);
            reg['A'] = num / den;
            return insPtr + 2;
        }

        int ExBxl(int insPtr)
        {
            long operand = sourceCode[insPtr + 1];
            reg['B'] = reg['B'] ^ operand;
            return insPtr + 2;
        }

        int ExBst(int insPtr)
        {
            long operand = ComboOperand(sourceCode[insPtr + 1]);
            reg['B'] = operand % 8;
            return insPtr + 2;
        }

        int ExJnz(int insPtr)
            => reg['A'] == 0 ? insPtr + 2 : sourceCode[insPtr + 1];

        int ExBxc(int insPtr)
        {
            reg['B'] = reg['B'] ^ reg['C'];
            return insPtr + 2;
        }

        int ExOut(int insPtr)
        {
            long operand = ComboOperand(sourceCode[insPtr + 1]);
            operand = operand % 8;
            outputBuffer.Add(operand);
            return insPtr + 2;
        }

        int ExBdv(int insPtr)
        {
            long operand = ComboOperand(sourceCode[insPtr + 1]);
            long num = reg['A'];
            long den = (int)Math.Pow(2, operand);
            reg['B'] = num / den;
            return insPtr + 2;
        }

        int ExCdv(int insPtr)
        {
            long operand = ComboOperand(sourceCode[insPtr + 1]);
            long num = reg['A'];
            long den = (int)Math.Pow(2, operand);
            reg['C'] = num / den;
            return insPtr + 2;
        }

        int ExIns(int insPtr)
            => sourceCode[insPtr] switch
            {
                Ins_Adv => ExAdv(insPtr),
                Ins_Bxl => ExBxl(insPtr),
                Ins_Bst => ExBst(insPtr),
                Ins_Jnz => ExJnz(insPtr),
                Ins_Bxc => ExBxc(insPtr),
                Ins_Out => ExOut(insPtr),
                Ins_Bdv => ExBdv(insPtr),
                Ins_Cdv => ExCdv(insPtr)
            };

        string RunProgram()
        {
            int insPtr = 0;

            while(insPtr<sourceCode.Count)
                insPtr = ExIns(insPtr);

            return string.Join(',', outputBuffer);
        }

        // Brute force has not been an option, left here for the laughs (notice that I've tried to speed up the loops with early breaks ;) (innocent me)
        string FindAutoReplica()
        {
            Dictionary<char, long> backup = new();
            backup['A'] = reg['A'];
            backup['B'] = reg['B'];
            backup['C'] = reg['C'];

            int num = 0;
            var target = string.Join(',', sourceCode);

            while (true)
            {
                num++;

                foreach (var key in backup.Keys)
                    reg[key] = backup[key];

                outputBuffer.Clear();
                reg['A'] = num;
                int insPtr = 0;

                while (insPtr < sourceCode.Count)
                {
                    insPtr = ExIns(insPtr);
                    if (outputBuffer.Count > sourceCode.Count)
                        break;

                    if (outputBuffer.Count > 1)
                        if (outputBuffer.Zip(sourceCode, (x, y) => x == y).Any(x => !x))
                            break;

                    if (outputBuffer.Count == sourceCode.Count)
                        if (outputBuffer.Zip(sourceCode, (x, y) => x == y).All(x => x))
                            break;

                }

                if (outputBuffer.Count == sourceCode.Count)
                    if (outputBuffer.Zip(sourceCode, (x, y) => x == y).All(x => x))
                        break;
            }

            return num.ToString();
        }

        List<long> Disassembly(long num)
        {
            List<long> retVal = new();

            // In c#
            long regA = num;
            long regB = 0;
            long regC = 0;
        SourceCodeStart:
            regB = regA % 8;            // 2,4 --> Bst ; combo(4) = regA
            regB = regB ^ 2;            // 1,2-- > Bxl B = B ^ 2
            regC = regA / (long)Math.Pow(2,regB);        // 7,5 --> cdv C = A >> combo (5) ; Combo(5) = regB
            regB = regB ^ regC;         // 4,1 --> bxc B = B XOR C                        
            regB = regB ^ 3;            // 1,3 --> bxl B = B XOR 1
            retVal.Add(regB % 8);   // 5,5 --> out regB ; Combo(5) = regB
            regA = regA >> 3;           // 0,3-- > Adv combo(3); A = A / 2 ^ 3
            if (regA != 0)
                goto SourceCodeStart;

            return retVal;
        }

        string part2()
        {
            // The tests below are to make sure what the program does. 
            // It takes only the last 3 bits of the input to generate an output
            // Then it combines it with some value of the last iteration to pick the following 3 bits and produce another digit
            // and so on

            // Test multiple input values to see how the program behaves
            var tst = Disassembly(0);
            tst = Disassembly(1);
            tst = Disassembly(2);
            tst = Disassembly(3);
            tst = Disassembly(4);
            tst = Disassembly(5);
            tst = Disassembly(6);
            tst = Disassembly(7);

            // Take into account that the program only uses the last 3 bits to produce an output
            tst = Disassembly(1 * 8 + 1);
            tst = Disassembly(1 * 8 + 2);
            tst = Disassembly(1 * 8 + 3);
            tst = Disassembly(1 * 8 + 4);
            tst = Disassembly(1 * 8 + 5);
            tst = Disassembly(1 * 8 + 6);
            tst = Disassembly(1 * 8 + 7);

            tst = Disassembly(2 * 8 + 1);
            tst = Disassembly(2 * 8 + 2);
            tst = Disassembly(2 * 8 + 3);
            tst = Disassembly(2 * 8 + 4);
            tst = Disassembly(2 * 8 + 5);
            tst = Disassembly(2 * 8 + 6);
            tst = Disassembly(2 * 8 + 7);

            // What we see is that we can build the whole output by looking for the valid inputs that generate our desired output
            // We should do it backwards, start by the last number, see which inputs give us that number, then pick the ones that do, 
            // product by 8 and check which ones give us the second, etc ..
            
            var found = false;
            List<long> candidates = [0];
            List<long> numsToTry = Enumerable.Range(0, 8).Select(x => (long) x).ToList();
           
            for (int i = sourceCode.Count-1; i >=0; i--)
            {
                if (i == 4)
                {
                    var a = 2;
                }
                List<int> targetInt = sourceCode[i..];
                var target = targetInt.Select(x => (long) x).ToList();
                List<long> newCandidates = [];
                foreach (var num in candidates)
                {
                    var attempts = numsToTry.Select(x => num * 8 + x).ToList();
                    var results = attempts.Select(x => Disassembly(x)).ToList();
                    var valid = attempts.Where(y => target.SequenceEqual(Disassembly(y))).ToList();
                    newCandidates.AddRange(valid);
                }
                candidates = newCandidates;
            }

            return candidates.Min().ToString();
        }

        public string Solve(int part = 1)
            => part == 1 ? RunProgram() : part2();
    }
}




/*
Dissambly: My input is 2,4,1,2,7,5,4,1,1,3,5,5,0,3,3,0

2,4 --> Bst A = A mod combo operand(4) => regA = reg A mod 8
1,2 --> adv A = A / combo (2)          => regA = reg A / 2^2 => reg A / 4
7,5 --> cdv C = A / combo (5)          => regC = reg A / 2^(regB)
4,1 --> bxc B = B XOR C                => regB = reg B ^ regC
1,3 --> bxl B = B XOR 1                => regB = reg B ^ 1
5,5 --> out combo(5) = out regB        => out regB
0,3 --> Adv combo(3) ; A = A/2^3       => A = A/8
3,0 --> jnz 0                          => if(A != 0) jump 0;

*/