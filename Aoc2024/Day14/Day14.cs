
using System.Text;

public class Day14
{
    private int _xLen;
    private int _yLen;
    private readonly char[,] _grid;

    private List<Robot> Robots;
    public Day14()
    {
        var mode = Mode.Input;
        string[] lines;
        if (mode == Mode.Example)
        {
            lines = File.ReadAllLines("Day14\\example.txt");
            _xLen = 11;
            _yLen = 7;
        }
        else
        {
            lines = File.ReadAllLines("Day14\\input.txt");
            _xLen = 101;
            _yLen = 103;
        }
        _grid = new char[_yLen, _xLen];

        for (int i = 0; i < _yLen; i++)
            for (int j = 0; j < _xLen; j++)
                _grid[i, j] = '.';

        Robots = new List<Robot>();

        foreach (var line in lines)
        {
            var splits = line.Replace("p=", "").Split(" v=");
            var posSplits = splits[0].Split(',');
            var velSplits = splits[1].Split(',');

            var r = new Robot(int.Parse(posSplits[0]), int.Parse(posSplits[1]), int.Parse(velSplits[0]), int.Parse(velSplits[1]));

            Robots.Add(r);
        }

    }

    public void Run()
    {

        var lim = 8160;
        for (int i = 0; i < lim; i++)
        {

            foreach (var robot in Robots)
            {
                robot.Move(_grid);
            }
            if(i>lim-5)
            Print(i);
        }
       
        var sum = GetSum();
    }


    public int GetSum()
    {


        var q1EndX = _xLen / 2;
        var q1EndY = _yLen / 2;

        var countQ1 = Robots.Count(r => r.Location.X < q1EndX && r.Location.Y < q1EndY);
        var countQ2 = Robots.Count(r => r.Location.X > q1EndX && r.Location.Y < q1EndY);
        var countQ3 = Robots.Count(r => r.Location.X < q1EndX && r.Location.Y > q1EndY);
        var countQ4 = Robots.Count(r => r.Location.X > q1EndX && r.Location.Y > q1EndY);


        return countQ1 * countQ2 * countQ3 * countQ4;
    }

    public void Print(int iteration)
    {
        var writer = File.AppendText("Day14\\output.txt");
        writer.WriteLine(iteration);
        for (int i = 0; i < _yLen; i++)
        {

            var s = "";
            for (int j = 0; j < _xLen; j++)
            {
                var count = Robots.Any(r => r.Location.Y == i && r.Location.X == j);
                if (count)
                    s += "#";
                else
                    s += '.';

            }

            writer.WriteLine(s);
        }

        writer.Close();
    }

}

public class Robot
{

    public Robot(int x, int y, int vel_x, int vel_y)
    {
        Location = new Coordinate(x, y);
        VelocityX = vel_x;
        VelocityY = vel_y;
    }
    public Coordinate Location { get; set; }
    public int VelocityX { get; set; }
    public int VelocityY { get; set; }

    internal void Move(char[,] grid)
    {
        var lim_y = grid.GetLength(0);
        var lim_x = grid.GetLength(1);


        if (VelocityY < 0)
        {
            for (int y = VelocityY; y < 0; y++)
            {
                Location.Y--;
                if (Location.Y < 0)
                    Location.Y = lim_y - 1;
            }
        }
        else if (VelocityY > 0)
        {
            for (int y = VelocityY; y > 0; y--)
            {
                Location.Y++;
                if (Location.Y >= lim_y)
                    Location.Y = 0;
            }
        }
        if (VelocityX < 0)
        {
            for (int x = VelocityX; x < 0; x++)
            {
                Location.X--;
                if (Location.X < 0)
                    Location.X = lim_x - 1;
            }
        }
        else if (VelocityX > 0)
        {
            for (int x = VelocityX; x > 0; x--)
            {
                Location.X++;
                if (Location.X >= lim_x)
                    Location.X = 0;
            }
        }



    }
}

public enum Mode
{
    Example,
    Input
}