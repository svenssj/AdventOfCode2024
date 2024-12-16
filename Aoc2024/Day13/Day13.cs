
public class Day13
{

    public List<ClawMachine> _machines;
    public Day13()
    {
        var lines = File.ReadAllLines("Day13\\input.txt");



        _machines = new List<ClawMachine>();
        var lim = lines.Count();
        for (int i = 0; i < lim; i++)
        {
            if (lines[i] == "")
                i++;
            var machine = new ClawMachine();
            var aString = lines[i].Replace("Button A: ", "").Split(", ");
            machine.A = new Button()
            {
                X = int.Parse(aString[0].Replace("X+", "")),
                Y = int.Parse(aString[1].Replace("Y+", ""))
            };
            i++;

            var bString = lines[i].Replace("Button B: ", "").Split(", ");

            machine.B = new Button()
            {
                X = int.Parse(bString[0].Replace("X+", "")),
                Y = int.Parse(bString[1].Replace("Y+", ""))
            };
            i++;

            var priceString = lines[i].Replace("Prize: ", "").Split(", ");
            machine.TargetX = int.Parse(priceString[0].Replace("X=", "")) + 10000000000000;
            machine.TargetY = int.Parse(priceString[1].Replace("Y=", "")) + 10000000000000;
            _machines.Add(machine);
        }

    }

    public void Run()
    {

        long sum = 0;

        foreach (var machine in _machines)
        {
            var tmp = machine.FetchPrize();
            if (tmp != -1)
                sum += tmp;
        }

    }

}

public class ClawMachine
{
    public Button A { get; set; }
    public Button B { get; set; }
    public long TargetX { get; set; }
    public long TargetY { get; set; }



    public long FetchPrize()
    {

        var noA = (TargetX * B.Y - TargetY * B.X) / (A.X * B.Y - A.Y * B.X);
        var noB = (TargetX * A.Y - TargetY * A.X) / (A.X * B.Y - A.Y * B.X) * -1;

        var runs = new List<Run>();

        long xval = noA * A.X + noB * B.X;
        long yval = noA * A.Y + noB * B.Y;

        if (xval == TargetX && yval == TargetY)
        {
            runs.Add(new Run() { ActivationsA = noA, ActivationsB = noB });
        }

        if (!runs.Any())
            return -1;

        return runs.Min(min => min.Cost);
    }

}

public class Button
{
    public int X { get; set; }
    public int Y { get; set; }

}
public class Run()
{
    public long ActivationsA { get; set; }
    public long ActivationsB { get; set; }
    public long Cost => ActivationsA * 3 + ActivationsB;

}

