public class Day24
{

    Dictionary<string, bool> Memory = [];
    List<Instruction> Instructions = [];


    public Day24()
    {

        var setupLines = File.ReadAllLines("Day24\\input_setup.txt");
        var instructionLines = File.ReadAllLines("Day24\\input.txt");

        foreach (var line in setupLines)
        {
            var splits = line.Split(": ");
            Memory.Add(splits[0], splits[1] == "1");
        }

        foreach (var line in instructionLines)
        {
            var splits = line.Split(" ");

            var op = OperatorEnum.Add;
            if (splits[1] == "AND")
                op = OperatorEnum.AND;
            else if (splits[1] == "OR")
                op = OperatorEnum.OR;
            else if (splits[1] == "XOR")
                op = OperatorEnum.XOR;
            else
            {
                throw new Exception("Incorrect operator");
            }

            var instruction = new Instruction()
            {
                A = splits[0],
                Operator = op,
                B = splits[2],
                C = splits[4]
            };


            Instructions.Add(instruction);

        }

    }

    public void Run()
    {

        while (!Instructions.All(i => i.Completed))
        {
            var remaining = Instructions.Where(i => !i.Completed);
            foreach (var instruction in remaining)
            {
                var result = false;
                var a = false;
                if (!Memory.ContainsKey(instruction.A))
                    continue;
                else
                    a = Memory[instruction.A];

                var b = false;
                if (!Memory.ContainsKey(instruction.B))
                    continue;
                else
                    b = Memory[instruction.B];

                switch (instruction.Operator)
                {
                    case OperatorEnum.AND:
                        result = AND(a, b);
                        break;
                    case OperatorEnum.OR:
                        result = OR(a, b);
                        break;
                    case OperatorEnum.XOR:
                        result = XOR(a, b);
                        break;
                }
                if (!Memory.ContainsKey(instruction.C))
                    Memory.Add(instruction.C, result);
                else
                    Memory[instruction.C] = result;
                instruction.Completed = true;
            }
        }

        var bits = Memory.Where(m => m.Key.StartsWith("z")).OrderBy(m => m.Key).Select(m => m.Value).Reverse().ToArray();

        if (bits == null)
            throw new Exception("incorrect memory");

        var answer = ToLong(bits);

        Console.WriteLine(answer);

    }
    private bool AND(bool A, bool B)
    {
        return A && B;
    }
    private bool OR(bool A, bool B)
    {
        return A || B;
    }
    private bool XOR(bool A, bool B)
    {
        return A != B;
    }

    public long ToLong(bool[] bits)
    {

        var s = "";
        foreach (var bit in bits)
        {
            s += bit ? "1" : "0";
        }

        var res = Convert.ToInt64(s, 2);
        return res;
    }
}

public class Instruction
{
    public required string A { get; set; }
    public required string B { get; set; }
    public required string C { get; set; }
    public required OperatorEnum Operator { get; set; }
    public bool Completed { get; set; }
}

