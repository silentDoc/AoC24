using AoC24.Common;
namespace AoC24.Day24
{
    enum NodeType
    { 
        Input = 0,
        Combo = 1
    }

    class Node
    {
        public string Id;
        public NodeType Type = NodeType.Input;
        public string Src1;
        public string Src2;
        public string Op;
        public int StartValue;

        public Node(string id, NodeType type, string src1, string src2, string op, int startValue)
        {
            Id = id;
            Type = type;
            Src1 = src1;
            Src2 = src2;
            Op = op;
            StartValue = startValue;
        }

        public int GetValue(Dictionary<string, Node> allNodes)
        {
            if (Type == NodeType.Input)
                return StartValue;

            return Op switch
            {
                "AND" => allNodes[Src1].GetValue(allNodes) == 1 && allNodes[Src2].GetValue(allNodes) == 1 ? 1 : 0,
                "OR"  => allNodes[Src1].GetValue(allNodes) == 1 || allNodes[Src2].GetValue(allNodes) == 1 ? 1 : 0,
                "XOR" => allNodes[Src1].GetValue(allNodes) != allNodes[Src2].GetValue(allNodes) ? 1 : 0,
                _ => throw new Exception("Invalid operand !" + Op)
            };
        }
    }

    internal class LogicCircuit
    {
        Dictionary<string, Node> allNodes = new();

        public void ParseInput(List<string> input)
        {
            var sections = ParseUtils.SplitBy(input, "");

            foreach(var inputWire in sections[0])
            {
                var parts = inputWire.Split(": ");
                int value = int.Parse(parts[1]);
                allNodes[parts[0]] = new(parts[0], NodeType.Input, "", "", "", value);
            }

            foreach (var comboWire in sections[1])
            {
                var parts = comboWire.Replace(" ->", "").Split(" ");
                allNodes[parts[3]] = new(parts[3], NodeType.Combo, parts[0], parts[2], parts[1], -1);
            }
        }

        long FindOutput()
        {
            var outputWires = allNodes.Keys.Where(x => x.StartsWith("z")).OrderByDescending(x => x).ToList();
            var values = outputWires.Select(x => allNodes[x].GetValue(allNodes)).ToList();
            var binaryString = string.Concat(values);
            return Convert.ToInt64(binaryString, 2);

        }

        public long Solve(int part = 1)
            => FindOutput();
        
    }
}
