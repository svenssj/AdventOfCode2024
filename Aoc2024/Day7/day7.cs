using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

public class Day7
{

    public Day7()
    {
        var Lines = File.ReadAllLines("Day7\\input.txt");

        var calibrations = new List<Calibration>();

        foreach (var line in Lines)
        {
            calibrations.Add(new Calibration(line));

        }

        long correct = 0;
        foreach (var calibration in calibrations)
        {
            var valid = calibration.CheckAnswer();
            if (valid)
                correct += calibration.Answer;
        }

    }

    public void Run()
    {

    }
}

public class Calibration
{

    public Calibration(string unparsed)
    {
        var splits = unparsed.Split(": ");
        Answer = long.Parse(splits[0]);
        var ints = splits[1].Split(' ').Select(s => long.Parse(s));

        Root = new Node(0, ints.ToArray(), OperatorEnum.Add);
    }

    public bool CheckAnswer()
    {
        var ends = new List<Node>();
        Root.GetDeadEnds(ends);

        if (ends.Any(e => e.Sum == Answer))
            return true;
        return false;

    }

    public long Answer { get; set; }
    Node Root;
}

public class Node
{

    public void GetDeadEnds(List<Node> list)
    {
        if (Mul == null)
            list.Add(this);
        else
        {
            Mul.GetDeadEnds(list);
            Add.GetDeadEnds(list);
            Comb.GetDeadEnds(list);
        }

    }

    public Node(long sum, long[] inputs, OperatorEnum op)
    {
        var first = inputs.First();
        var remaining = inputs.Skip(1).ToArray();

        Value = first;

        if (op == OperatorEnum.Add)
            sum = sum + Value;
        else if (op == OperatorEnum.Multiply)
            sum = sum * Value;
        else
        {
            sum = long.Parse(sum.ToString() + Value.ToString());
        }


        Sum = sum;

        if (remaining.Length > 0)
        {
            Mul = new Node(sum, remaining, OperatorEnum.Multiply);
            Add = new Node(sum, remaining, OperatorEnum.Add);
            Comb = new Node(sum, remaining, OperatorEnum.Comb);
        }
    }
    public OperatorEnum Operator { get; set; }
    public long Sum { get; set; }
    long Value { get; set; }
    Node? Mul { get; set; }
    Node? Add { get; set; }
    Node? Comb { get; set; }
}

public enum OperatorEnum
{
    Add,
    Multiply,
    Comb,
    AND,
    OR,
    XOR
}