using AoC24.Common;

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

        Dictionary<char, int> reg = new();
        List<int> sourceCode = new();
        List<int> outputBuffer = new();

        public void ParseInput(List<string> lines)
        {
            reg['A'] = int.Parse(lines[0].Replace("Register A: ", ""));
            reg['B'] = int.Parse(lines[1].Replace("Register B: ", ""));
            reg['C'] = int.Parse(lines[2].Replace("Register C: ", ""));
            sourceCode = lines[4].Replace("Program: ", "").Split(',').Select(int.Parse).ToList();
        }

        int ComboOperand(int operand)
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
            int operand = ComboOperand(sourceCode[insPtr + 1]);
            int num = reg['A'];
            int den = (int) Math.Pow(2,operand);
            reg['A'] = num / den;
            return insPtr + 2;
        }

        int ExBxl(int insPtr)
        {
            int operand = sourceCode[insPtr + 1];
            reg['B'] = reg['B'] ^ operand;
            return insPtr + 2;
        }

        int ExBst(int insPtr)
        {
            int operand = ComboOperand(sourceCode[insPtr + 1]);
            reg['B'] = MathHelper.Modulo(operand,8);
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
            int operand = ComboOperand(sourceCode[insPtr + 1]);
            operand = MathHelper.Modulo(operand, 8);
            outputBuffer.Add(operand);
            return insPtr + 2;
        }

        int ExBdv(int insPtr)
        {
            int operand = ComboOperand(sourceCode[insPtr + 1]);
            int num = reg['A'];
            int den = (int)Math.Pow(2, operand);
            reg['B'] = num / den;
            return insPtr + 2;
        }

        int ExCdv(int insPtr)
        {
            int operand = ComboOperand(sourceCode[insPtr + 1]);
            int num = reg['A'];
            int den = (int)Math.Pow(2, operand);
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

        public string Solve(int part = 1)
            => RunProgram();
    }
}
