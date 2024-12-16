using System.Net.Mail;
using Microsoft.VisualBasic;

public class Day15
{

    private int x_len;
    private int y_len;
    private char[,] _grid;
    private string instructions = "";
    public Day15()
    {

        var lines = File.ReadAllLines("Day15\\input.txt");

        var map = true;
        var maplines = new List<string>();
        foreach (var line in lines)
        {
            if (line == "")
            {
                map = false;
                continue;
            }
            if (map)
                maplines.Add(line);
            else
                instructions += line;
        }

        maplines = Scale(maplines);

        x_len = maplines[0].Length;
        y_len = maplines.Count;
        _grid = new char[y_len, x_len];
        for (int i = 0; i < y_len; i++)
        {
            for (int j = 0; j < x_len; j++)
            {
                _grid[i, j] = maplines[i][j];
            }
        }
        Print();

    }

    private List<string> Scale(List<string> map)
    {
        var newMap = new List<string>();

        foreach (var line in map)
        {
            newMap.Add(line.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@."));
        }

        return newMap;
    }



    public void Run()
    {

        var start = ExtractStart('@');

        var i = 0;
        //
        foreach (var step in instructions)
        // var k = Console.Read();
        // while (k != 'x')
        {
            // char step = 'q';
            // if (k == 'w')
            //     step = '^';
            // else if (k == 's')
            //     step = 'v';
            // else if (k == 'd')
            //     step = '>';
            // else if (k == 'a')
            //     step = '<';
            if (step == '^')
                if (AttemptMove(start.X, start.Y, DirectionEnum.North))
                    start.Y--;
            if (step == '>')
                if (AttemptMove(start.X, start.Y, DirectionEnum.East))
                    start.X++;
            if (step == '<')
                if (AttemptMove(start.X, start.Y, DirectionEnum.West))
                    start.X--;
            if (step == 'v')
                if (AttemptMove(start.X, start.Y, DirectionEnum.South))
                    start.Y++;
            Console.WriteLine($"Step {i} - {step}");

            if (i == 140)
                Console.WriteLine("Bug!");

         //   Print();
            // Thread.Sleep(250);
            // k = Console.Read();
            i++;
        }
        var score = Score(true);
        //1460702 low
    }

    public long Score(bool p2)
    {
        long sum = 0;
        for (int i = 0; i < y_len; i++)
        {

            for (int j = 0; j < x_len; j++)
            {
                var c = _grid[i, j];
                if (!p2)
                {
                    if (c == 'O')
                        sum += i * 100 + j;
                }
                else
                 if (c == '[')
                    sum += i * 100 + j;
            }

        }

        return sum;
    }

    private bool AttemptMove(int x, int y, DirectionEnum direction)
    {
        var o_x = x;
        var o_Y = y;
        switch (direction)
        {
            case DirectionEnum.North:
                y--;
                break;
            case DirectionEnum.East:
                x++;
                break;
            case DirectionEnum.South:
                y++;
                break;
            case DirectionEnum.West:
                x--;
                break;

        }

        var destination = _grid[y, x];
        if (destination == '#')
            return false;
        if (destination == '.')
        {
            var o = _grid[o_Y, o_x];
            _grid[y, x] = o;
            _grid[o_Y, o_x] = '.';
            return true;
        }
        if (destination == 'O')
        {
            var pushresult = AttemptMove(x, y, direction);
            if (!pushresult)
                return false;
            var o = _grid[o_Y, o_x];
            _grid[y, x] = o;
            _grid[o_Y, o_x] = '.';
            return true;
        }
        if ((direction == DirectionEnum.East || direction == DirectionEnum.West) && (destination == '[' || destination == ']'))
        {

            var pushresult = AttemptMove(x, y, direction);
            if (!pushresult)
                return false;
            var o = _grid[o_Y, o_x];
            _grid[y, x] = o;
            _grid[o_Y, o_x] = '.';
            return true;
        }
        else if (direction == DirectionEnum.North || direction == DirectionEnum.South)
        {
            if (destination == '[')
            {
                var moveLeft = CanIMoveBigBoxUpDown(x, o_Y, direction);
                var moveRight = CanIMoveBigBoxUpDown(x + 1, o_Y, direction);
                if (!(moveLeft && moveRight))
                    return false;


                var o = _grid[o_Y, o_x];
                SetPosToVal(x + 1, y, '.', direction);
                SetPosToVal(x, y, o, direction);
                _grid[y, x] = o;
                _grid[o_Y, o_x] = '.';

                return true;
            }
            if (destination == ']')
            {
                var moveLeft = CanIMoveBigBoxUpDown(x, y, direction);
                var moveRight = CanIMoveBigBoxUpDown(x - 1, y, direction);
                if (!(moveLeft && moveRight))
                    return false;
                var o = _grid[o_Y, o_x];
                SetPosToVal(x, y, o, direction);
                SetPosToVal(x - 1, y, '.', direction);
                _grid[y, x] = o;
                _grid[o_Y, o_x] = '.';


                return true;
            }
        }

        throw new NotImplementedException("unknown char");
    }

    private bool CanIMoveBigBoxUpDown(int x, int y, DirectionEnum direction)
    {
        if (direction == DirectionEnum.North)
        {
            y--;

        }
        else
        {
            y++;
        }

        var val = _grid[y, x];
        if (val == '.')
            return true;
        if (val == '#')
            return false;
        if (val == '[')
        {
            return CanIMoveBigBoxUpDown(x, y, direction) && CanIMoveBigBoxUpDown(x + 1, y, direction);
        }
        if (val == ']')
        {
            return CanIMoveBigBoxUpDown(x, y, direction) && CanIMoveBigBoxUpDown(x - 1, y, direction);
        }
        throw new Exception("");
    }
    private void SetPosToVal(int x, int y, char val, DirectionEnum direction)
    {
        var nextY = 0;

        if (direction == DirectionEnum.North)
        {
            nextY = y - 1;
        }
        else
        {
            nextY = y + 1;
        }



        var location = _grid[y, x];
        if (location == '#')
            throw new Exception();

        if (location == ']')
        {
            SetPosToVal(x, nextY, location, direction);
            _grid[y, x - 1] = '.';
            SetPosToVal(x - 1, nextY, '[', direction);

        }
        if (location == '[')
        {
            SetPosToVal(x, nextY, location, direction);
            _grid[y, x + 1] = '.';
            SetPosToVal(x + 1, nextY, ']', direction);

        }

        if (location != '.')
        {
            if (val == ']')
            {

            }
            if (val == '[')
            {

            }
        }
        _grid[y, x] = val;

    }

    private Coordinate ExtractStart(char startChar)
    {


        for (int i = 0; i < y_len; i++)
        {
            for (int j = 0; j < x_len; j++)
            {
                if (_grid[i, j] == startChar)
                    return new Coordinate(j, i) { };
            }
        }

        throw new Exception("Missing start");
    }

    private void Print()
    {
        Console.WriteLine(" 0123456789");
        for (int i = 0; i < y_len; i++)
        {
            var s = "";
            for (int j = 0; j < x_len; j++)
            {
                s += _grid[i, j];
            }
            Console.WriteLine(i + s);
        }
    }
}