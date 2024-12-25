using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net.Mail;
using System.Numerics;
using Microsoft.VisualBasic;

public class Day21
{

    private List<string> _codes;

    private Location[,] _numpad = new Location[4, 3];
    private Location[,] _dirpad = new Location[2, 3];

    private bool PRINT_BOOL = false;


    Location _numpadPos;
    Location _directionalControllerDirpadPos;
    Location _robotDirpadPos;

    public Day21()
    {
        _codes = File.ReadAllLines("Day21\\example.txt").ToList();

        _numpad[0, 0] = new Location() { Value = '7', X = 0, Y = 0 };
        _numpad[0, 1] = new Location() { Value = '8', X = 1, Y = 0 };
        _numpad[0, 2] = new Location() { Value = '9', X = 2, Y = 0 };
        _numpad[1, 0] = new Location() { Value = '4', X = 0, Y = 1 };
        _numpad[1, 1] = new Location() { Value = '5', X = 1, Y = 1 };
        _numpad[1, 2] = new Location() { Value = '6', X = 2, Y = 1 };
        _numpad[2, 0] = new Location() { Value = '1', X = 0, Y = 2 };
        _numpad[2, 1] = new Location() { Value = '2', X = 1, Y = 2 };
        _numpad[2, 2] = new Location() { Value = '3', X = 2, Y = 2 };
        _numpad[3, 0] = new Location() { Value = 'X', X = 0, Y = 3 };
        _numpad[3, 1] = new Location() { Value = '0', X = 1, Y = 3 };
        _numpad[3, 2] = new Location() { Value = 'A', X = 2, Y = 3 };

        _dirpad[0, 0] = new Location() { Value = 'X', X = 0, Y = 0 };
        _dirpad[0, 1] = new Location() { Value = '^', X = 1, Y = 0 };
        _dirpad[0, 2] = new Location() { Value = 'A', X = 2, Y = 0 };
        _dirpad[1, 0] = new Location() { Value = '<', X = 0, Y = 1 };
        _dirpad[1, 1] = new Location() { Value = 'v', X = 1, Y = 1 };
        _dirpad[1, 2] = new Location() { Value = '>', X = 2, Y = 1 };

        Print(_numpad);

        Print(_dirpad);

        _numpadPos = new Location() { X = 2, Y = 3, Value = '*' };
        _robotDirpadPos = new Location() { X = 2, Y = 0, Value = '*' };
        _directionalControllerDirpadPos = new Location() { X = 2, Y = 0, Value = '*' };
    }

    public static void Print(Location[,] grid)
    {

        var _yLen = grid.GetLength(0);
        var _xLen = grid.GetLength(1);

        Console.WriteLine("");
        for (int i = 0; i < _yLen; i++)
        {
            var s = "";
            for (int j = 0; j < _xLen; j++)
            {

                s += grid[i, j].Value;


            }
            Console.WriteLine(s);
        }
        Console.WriteLine("");
    }

    public void Run()
    {

         _codes = ["029A"];
        long res = 0;
        foreach (var code in _codes)
        {
            var intCode = int.Parse(code.Substring(0, 3));
            var trail = PressNextNum(code);
            //   Console.WriteLine(trail);
            var tmp = trail.Length * intCode;
            res += tmp;
            Console.WriteLine($"{tmp}={trail.Length}*{intCode}");
        }

        Console.WriteLine(res);
    }


    public string PressNextNum(string code)
    {

        // _numpadPos = new Coordinate(2, 3);
        // _robotDirpadPos = new Coordinate(2, 0);
        // _directionalControllerDirpadPos = new Coordinate(2, 0);
        if (code == "")
            return "";

        var numRobotInstr = "";
        var dirRobotInsr = "";
        var dirdirBotInstr = "";
        foreach (var number in code)
        {
            if (PRINT_BOOL)
                Console.WriteLine(number);
            var numVector = GetMoveVector(KeypadEnum.Numeric, number);
            if (PRINT_BOOL)
                Console.WriteLine("num: " + numVector);
           

            numRobotInstr += numVector;
        }
         foreach (var a in numRobotInstr)
        {
                var dirInstr = GetMoveVector(KeypadEnum.DirectionalRobot, a);
                if (PRINT_BOOL)
                    Console.WriteLine("dir: " + dirInstr);
            
                dirRobotInsr += dirInstr;
        }
        foreach (var b in dirRobotInsr)
            {
                    var dirdirvector = GetMoveVector(KeypadEnum.DirectionalDirectionalRobot, b);
                    if (PRINT_BOOL)
                        Console.WriteLine("dirdir: " + dirdirvector);
                    dirdirBotInstr += dirdirvector;
            }



        
            Console.WriteLine(dirdirBotInstr);
            Console.WriteLine(dirRobotInsr);
            Console.WriteLine(numRobotInstr);
            Console.WriteLine(code);
        


        return dirdirBotInstr;
    }

    public string GetMoveVector(KeypadEnum keypad, char target)
    {
        var xDiff = 0;
        var yDiff = 0;
        var movestring = "";
        Location[,] grid;
        if (keypad == KeypadEnum.Numeric)
            grid = _numpad;
        else
            grid = _dirpad;

        Location finger = null;
        Location targetCoordinate = null;
        if (keypad == KeypadEnum.Numeric)
        {
            targetCoordinate = GetCoordinate(grid, target);
            finger = _numpadPos;
        }
        if (keypad == KeypadEnum.DirectionalRobot)
        {
            targetCoordinate = GetCoordinate(grid, target);
            finger = _robotDirpadPos;
        }
        if (keypad == KeypadEnum.DirectionalDirectionalRobot)
        {
            targetCoordinate = GetCoordinate(grid, target);
            finger = _directionalControllerDirpadPos;
        }

        if (finger == null || targetCoordinate == null)
            throw new Exception();

        xDiff = targetCoordinate.X - finger.X;
        yDiff = targetCoordinate.Y - finger.Y;






        char yChar = 'v';
        char xChar = '>';
        var xMoves = xDiff;
        var yMoves = yDiff;
        var xMod = 1;
        var yMod = 1;

        if (xMoves == -2)
        {
            xChar = '<';
            xMoves = xMoves * -1;
            xMod = -1;
        }
        else if (xMoves < 0)
        {
            xChar = '<';
            xMoves = xMoves * -1;
            xMod = -1;
        }

        if (yMoves < 0)
        {
            yChar = '^';
            yMoves = yMoves * -1;
            yMod = -1;
        }


        bool hasFailsafe = true;
        for (int i = 0; i < xMoves; i++)
        {
            if (i == 1 && hasFailsafe && xDiff == -2)
            {
                yMoves--;
                movestring += yChar;
                hasFailsafe=false;
            }
            finger.X += xMod;
            movestring += xChar;
        }

        for (int i = 0; i < yMoves; i++)
        {

            finger.Y += yMod;
            //    Console.WriteLine($"finger : [{finger.Y},{finger.X}] = {grid[finger.Y, finger.X].Value}");
            movestring += yChar;
        }





        if (xDiff == -2)
            return movestring + 'A';
        return movestring + 'A';
    }

    public Location GetCoordinate(Location[,] grid, char target)
    {



        var _yLen = grid.GetLength(0);
        var _xLen = grid.GetLength(1);
        for (int i = 0; i < _yLen; i++)
        {

            for (int j = 0; j < _xLen; j++)
            {

                if (target == grid[i, j].Value)
                    return grid[i, j];
            }

        }

        throw new KeyNotFoundException();
    }
}

public enum KeypadEnum
{
    Numeric,
    DirectionalRobot,

    DirectionalDirectionalRobot
}