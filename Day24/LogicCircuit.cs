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

        Node? FindNode(string src1, string src2, string op)
        {
            // We do not know the order in which the wires are put in the inputs
            var res = allNodes.Values.Where(n => n.Src1 == src1 && n.Src2 == src2 && n.Op == op).FirstOrDefault();
            if(res is null)
                res = allNodes.Values.Where(n => n.Src1 == src2 && n.Src2 == src1 && n.Op == op).FirstOrDefault();
            return res;
        }

        Node? FindNode(string src, string op)
            => allNodes.Values.Where(n => (n.Src1 == src || n.Src2 == src) && n.Op == op).FirstOrDefault();

        string FindWiresToSwap()
        {
            // Generate something I can visualize with Dot
            var xPins = allNodes.Keys.Where(k => k.StartsWith("x")).ToList();
            var yPins = allNodes.Keys.Where(k => k.StartsWith("y")).ToList();

            var opNodes = allNodes.Keys.Where(k => !k.StartsWith("x") && !k.StartsWith("y")).ToList();

            List<string> dotSrc = new();

            dotSrc.Add("digraph G {");
            foreach (var k in opNodes)
            {
                var n = allNodes[k];
                var op = (n.Src1 + "_" + n.Op + "_" + n.Src2);
                var color = n.Op switch
                {
                    "AND" => "yellow",
                    "OR" => "green",
                    "XOR" => "cyan",
                };
                dotSrc.Add(op + " [shape=box, style=filled, color=" + color +"]");
                dotSrc.Add(n.Src1 + " -> " + op + " -> " + n.Id);
                dotSrc.Add(n.Src2 + " -> " + op);
            }
            dotSrc.Add("}");

            // We have the graph in fullGraph to copypaste and visualize with Dot/GraphViz
            // so we can at least start looking for what to change
            var fullGraph = string.Join('\n', dotSrc);

            // Now some research - The problem states that we have a circuit that is trying to add numbers X and Y
            // A first google search led me to a wiki page and this stuff
            // https://www.google.com/search?q=electronic+circuit+to+add+numbers
            // https://en.wikipedia.org/wiki/Adder_(electronics)
            // Now back to my university years (I hated hardware btw:P) - but I kind of remember the difference between HALF ADDERs and FULL ADDERs
            // Half adders do NOT have a carry input - but since we are trying to sum multiple digit inputs, we have to use FULL ADDERS
            // https://en.wikipedia.org/wiki/Adder_(electronics)#/media/File:Full-adder_logic_diagram.svg
            // 
            // From here, we can derive that in our problem, we will have blocks that do this:
            //
            // (x,y,carry) => (z, newCarry) (https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/1-bit_full-adder.svg/1280px-1-bit_full-adder.svg.png)
            // 
            // 1 - Let t1 = x XOR y
            // 2 - Let t2 = x AND B
            // 3 - Let z = t1 XOR carry
            // 4 - Let t3 = t1 AND carry
            // 5 - Let newCarry = t2 OR t3

            // A visual inspection of the graph tells me that we're on the right track. (the code to generate DOT file and the pngs are attached to the solution)
            // The first block (x0, y0) works like a HALF ADDER because it does not have any carry as an input. But from (x1, y1) onwards, we should have be able to identify the wires
            // Let's try to go through the whole circuit starting from (x1, y1, c1) and identify for each digit if they stick to the circuit or not

            var xs = allNodes.Keys.Where(k => k.StartsWith("x") && k!="x00").OrderBy(k => k).ToList();
            var ys = allNodes.Keys.Where(k => k.StartsWith("y") && k != "y00").OrderBy(k => k).ToList();
            var pairs = xs.Zip(ys).ToList();
            Dictionary<int, string> carries = new();

            // We will keep a reference of the wires that hold the carries

            var carry = FindNode("x00", "y00", "AND");
            if (carry is not null)
                carries[0] = carry.Id;

            List<string> result = [];

            foreach (var pair in pairs)     // From the namings in the comments above
            {
                var nX = pair.First;
                var nY = pair.Second;
                var level = int.Parse(nX.Substring(1));
                var inCarry = carries[level - 1];

                var t1 = FindNode(nX, nY, "XOR");
                var t2 = FindNode(nX, nY, "AND");

                if (level == 19)
                {
                    Console.WriteLine(level.ToString() + " - cph,z19");
                    result.AddRange(["cph","z19"]);
                    carries[level] = "crr";
                    continue;
                }

                if (level == 33)
                {
                    Console.WriteLine(level.ToString() + " - z33,hgj");
                    result.AddRange(["z33", "hgj"]);
                    carries[level] = "wvn";
                    continue;
                }

                var z = FindNode(t1.Id, inCarry, "XOR");
                var t3 = FindNode(t1.Id, inCarry, "AND");

                // First approach, semi assisted solution - tell me the levels where we have problems with the related node names
                // I solve the problem with this code and visual inspection of the graph (the best I can come up for now)
                if (z == null || t3 == null)
                {
                    // We output the level
                    List<string> names = [];
                    if (t1 is not null)
                        names.Add(t1.Id);
                    if (t2 is not null)
                        names.Add(t2.Id);
                    if (t3 is not null)
                        names.Add(t3.Id);
                    result.AddRange(names);
                    Console.WriteLine(level.ToString() + " - " + string.Join(',', names));
                    // But we have to find the carry anyway before continuing
                    var brokenCarry = FindNode(t2.Id, "OR") is not null ? FindNode(t2.Id, "OR") : FindNode(t1.Id, "OR");
                    carries[level] = brokenCarry.Id;
                    continue;
                }

                var outCarry = FindNode(t2.Id, t3.Id, "OR");
                carries[level] = outCarry.Id;
            }

            return string.Join(',',result.OrderBy(x=>x));
        }

        public string Solve(int part = 1)
            => part == 1 ? FindOutput().ToString() : FindWiresToSwap();
        
    }
}
