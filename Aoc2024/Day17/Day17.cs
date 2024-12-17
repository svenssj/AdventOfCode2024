using System.Collections;

public class InitialState
{
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }
    public required string InitString { get; set; }
}

public class Day17
{

    int A { get; set; }
    int B { get; set; }
    int C { get; set; }


    List<int> instructions;

    public int InstructionPointer { get; set; }

    public Day17(InitialState state)
    {

        A = state.A;
        B = state.A;
        C = state.A;
        instructions = GetInstructions(state.InitString);

        // A = 729;
        // instructions = GetInstructions("0,1,5,4,3,0");
        //4,6,3,5,6,3,5,2,1,0 OK


        // C = 9;
        // instructions = GetInstructions("2,6");
        //B=1 OK

        // A = 10;
        // instructions = GetInstructions("5,0,5,1,5,4");
        //0,1,2 OK

        // A = 2024;
        // instructions = GetInstructions("0,1,5,4,3,0");
        //4,2,5,6,7,7,7,7,3,1,0 , A = 0 OK

        // B = 29;
        // instructions = GetInstructions("1,7");
        //B = 26 OK

        // var x = ToInt([false, false, false, true]);
        // x = ToInt([true, false, false, true]);

        // B = 2024;
        // C = 43690;
        // instructions = GetInstructions("4,0");
        //B = 44354
    }

    private List<int> GetInstructions(string s)
    {
        return s.Split(',').Select(int.Parse).ToList();
    }

    public string Run()
    {

        var len = instructions.Count;
        InstructionPointer = 0;

        var result = new List<int>();
        while (InstructionPointer < len)
        {

            var opcode = instructions[InstructionPointer];
            var operand = instructions[InstructionPointer + 1];
            switch (opcode)
            {
                case 0:
                    Adv(operand);
                    InstructionPointer += 2;
                    break;
                case 1:
                    Bxl(operand);
                    InstructionPointer += 2;
                    break;
                case 2:
                    Bst(operand);
                    InstructionPointer += 2;
                    break;
                case 3:
                    var jumped = Jnz(operand);
                    if (!jumped)
                        InstructionPointer += 2;

                    break;
                case 4:
                    Bxc(operand);
                    InstructionPointer += 2;
                    break;
                case 5:
                    var output = Out(operand);
                    result.Add(output);
                    InstructionPointer += 2;
                    break;
                case 6:
                    Bdv(operand);
                    InstructionPointer += 2;
                    break;
                case 7:
                    Cdv(operand);
                    InstructionPointer += 2;
                    break;
            }
        }

        return string.Join(',', result);
    }

    //opcode 0
    public void Adv(int combo)
    {

        A = int.Parse((A / (Math.Pow(2, GetVal(combo)))).ToString().Split(',')[0]);
    }
    //opcode 1
    public void Bxl(int literal)
    {
        var literalBits = ToBits(literal);

        var bBits = ToBits(B);

        if (literalBits.Length < bBits.Length)
        {
            var diff = bBits.Length - literalBits.Length;
            for (int i = 0; i < diff; i++)
            {
                literalBits.Prepend(false);
            }
        }
        if (bBits.Length < literalBits.Length)
        {
            var diff = literalBits.Length - bBits.Length;
            for (int i = 0; i < diff; i++)
            {
                bBits.Prepend(false);
            }
        }

        var res = new List<bool>();

        var len = literalBits.Length;


        for (int i = 0; i < len; i++)
        {
            if (bBits[i] != literalBits[i])
                res.Add(true);
            else
                res.Add(false);
        }
        B = ToInt(res.ToArray());
    }
    //opcode 2
    public void Bst(int combo)
    {
        B = GetVal(combo) % 8;
    }
    //opcode 3
    public bool Jnz(int lit)
    {
        if (A == 0)
            return false;
        InstructionPointer = lit;
        return true;

    }
    //opcode 4
    public void Bxc(int code)
    {
        var bBits = ToBits(B);
        var cBits = ToBits(C);

        var res = new List<bool>();

        var len = bBits.Length;


        for (int i = 0; i < len; i++)
        {
            if (bBits[i] != cBits[i])
                res.Add(true);
            else
                res.Add(false);
        }
        B = ToInt(res.ToArray());

    }
    //opcode 5
    public int Out(int combo)
    {
        return GetVal(combo) % 8;
    }
    //opcode 6
    public void Bdv(int combo)
    {
        B = int.Parse((A / (Math.Pow(2, GetVal(combo)))).ToString().Split(',')[0]);
    }
    //opcode 7
    public void Cdv(int combo)
    {
        C = int.Parse((A / (Math.Pow(2, GetVal(combo)))).ToString().Split(',')[0]);
    }



    public int GetVal(int opcode)
    {
        if (opcode >= 0 && opcode <= 3)
            return opcode;
        else if (opcode == 4)
            return A;
        else if (opcode == 5)
            return B;
        else if (opcode == 6)
            return C;
        throw new Exception("Invalid op code");
    }

    public bool[] ToBits(int i)
    {
        return new BitArray(new int[] { i }).Cast<bool>().Reverse().ToArray();
    }

    public int ToInt(bool[] bits)
    {

        var len = bits.Length - 1;
        var res = 0;
        for (int i = len; i >= 0; i--)
        {
            if (bits[i])
                res += int.Parse(Math.Pow(2, (len - i)).ToString());
        }

        return res;
    }
}