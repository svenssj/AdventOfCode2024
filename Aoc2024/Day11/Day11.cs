using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Sockets;
using Microsoft.VisualBasic;

public class Day11
{


    List<Stone> stones;
    Dictionary<int, Dictionary<long, long>> GenerationsScaleDict;
    public Day11()
    {
        var line = File.ReadAllText("Day11\\input.txt");

        GenerationsScaleDict = new Dictionary<int, Dictionary<long, long>>();
        stones = new List<Stone>();
        var splits = line.Split(" ");
        foreach (var split in splits)
        {
            stones.Add(new Stone(long.Parse(split), GenerationsScaleDict));
        }
    }

    public void Run()
    {
        var n = 75;
        long sum = 0;
        var sw = new Stopwatch();
        sw.Start();
        foreach (var stone in stones)
        {
            var x = stone.Blink(0, n);
            Console.WriteLine(x);
            sum += x;
        }
        sw.Stop();
        Console.WriteLine("Elapsed ms: " + sw.ElapsedMilliseconds);
        Console.WriteLine("Total: " + sum);

        long s = 0;

        foreach(var kvp in GenerationsScaleDict)
        {
            var valuedict = kvp.Value;
        
            foreach(var innerKvp in valuedict)
            {
                s ++;
            }
        }



    }
}

public class Stone
{
    Dictionary<int, Dictionary<long, long>> LevelValueRemainingDict;
    public Stone(long value, Dictionary<int, Dictionary<long, long>> dict)
    {
        Value = value;
        LevelValueRemainingDict = dict;
    }

    public long Value { get; set; }
    private long? Override { get; set; }

    public Stone? Left { get; set; }
    public Stone? Right { get; set; }

    public long Blink(int current, int limit)
    {
        if (current == limit)
            return 1;

        if (!LevelValueRemainingDict.ContainsKey(current))
            LevelValueRemainingDict.Add(current, []);

        //Have I been here before?
        var ValueRemainingDict = LevelValueRemainingDict[current];
        if (ValueRemainingDict.ContainsKey(Value))
        {
            return ValueRemainingDict[Value];
        }

        if (Left != null && Right != null)
        {
            var left = Left.Blink(current + 1, limit);
            var right = Right.Blink(current + 1, limit);
            return left + right;
        }
        else
        {
            var stoneString = Value.ToString();
            var len = stoneString.Length;
            if (Value == 0)
            {
                Value = 1;
                return Blink(current + 1, limit);
            }
            else if (len % 2 == 0)
            {
                Left = new Stone(long.Parse(stoneString.Substring(0, len / 2)), LevelValueRemainingDict);
                Right = new Stone(long.Parse(stoneString.Substring(len / 2, len / 2)), LevelValueRemainingDict);
                var tmp = Left.Blink(current + 1, limit) + Right.Blink(current + 1, limit);
                ValueRemainingDict.Add(Value, tmp);

                return tmp;
            }
            else
            {
                var currval = Value;

                Value *= 2024;
                var result = Blink(current + 1, limit);
                ValueRemainingDict.Add(currval, result);
                return result;
            }

        }


    }

    public long GetDepth()
    {
        if (Override.HasValue)
            return Override.Value;
        if (Left != null && Right != null)
        {
            var val = Left.GetDepth() + Right.GetDepth();

            return val;
        }
        return 1;
    }

    public void Flatten(List<Stone> list)
    {
        if (Left != null && Right != null)
        {
            Left.Flatten(list);
            Right.Flatten(list);
        }
        else
            list.Add(this);
    }
}

