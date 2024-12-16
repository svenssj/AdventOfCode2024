

using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

public static class Printer
{

    public static void Print(char[,] grid, List<Coordinate> trail)
    {

        var _yLen = grid.GetLength(0);
        var _xLen = grid.GetLength(1);

        Console.WriteLine("");
        for (int i = 0; i < _yLen; i++)
        {
            var s = "";
            for (int j = 0; j < _xLen; j++)
            {
                var existing = trail.SingleOrDefault(c => c.Y == i && c.X == j);
                if (existing != null)
                    s += PrintVisited(existing.VisitedWhenWalking);
                else
                    s += grid[i, j];


            }
            Console.WriteLine(s);
        }
        Console.WriteLine("");
    }

    private static char PrintVisited(DirectionEnum direction)
    {
        switch (direction)
        {
            case DirectionEnum.North:
                return '^';
            case DirectionEnum.East:
                return '>';
            case DirectionEnum.South:
                return 'v';
            case DirectionEnum.West:
                return '<';

        }
        throw new NotImplementedException();
    }
}

public class Day6
{
    private int _xLen;
    private int _yLen;
    private readonly char[,] _chargrid;

    public Day6()
    {
        var lines = File.ReadAllLines("Day6\\input.txt");
        _xLen = lines[0].Length;
        _yLen = lines.Length;

        var chargrid = new char[_yLen, _xLen];

        for (int i = 0; i < _yLen; i++)
        {
            for (int j = 0; j < _xLen; j++)
            {
                chargrid[i, j] = lines[i][j];
            }
        }
        _chargrid = chargrid;

    }

    public async Task<bool> Run()
    {
        var runner = new GridRunner(_chargrid);
        var initialRun = runner.Run();


        Printer.Print(_chargrid, initialRun);
        initialRun.Remove(initialRun.First());


        var tasks = new List<Task<bool>>();
        var sw = new Stopwatch();
        sw.Start();
        foreach (var possibleInterceptionPoint in initialRun)
        {
            tasks.Add(Task.Run(() => IsLooping(_chargrid, possibleInterceptionPoint)));
        }

        await Task.WhenAll(tasks);
        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
        var count = 0;
        foreach (var task in tasks)
        {
            if (task.Result == true)
                count++;
        } 
        Console.WriteLine(count);
        return true;
    }

    private bool IsLooping(char[,] chargrid, Coordinate possibleInterceptionPoint)
    {

        var alternateUniverse = chargrid.Clone() as char[,];
        if (alternateUniverse == null)
            throw new Exception();

        alternateUniverse[possibleInterceptionPoint.Y, possibleInterceptionPoint.X] = '#';

        var newRunner = new GridRunner(alternateUniverse);

        newRunner.Run();

        return newRunner.CircularExit;
    }

}

public class GridRunner
{
    private int _xLen;
    private int _yLen;
    private readonly char[,] _chargrid;
    private List<Coordinate> _trail;
    public bool CircularExit;

    public GridRunner(char[,] chargrid)
    {

        _chargrid = chargrid;
        _xLen = chargrid.GetLength(1);
        _yLen = chargrid.GetLength(0);
        _trail = [];
    }

    public List<Coordinate> Run()
    {
        var direction = DirectionEnum.North;

        //p1
        var start = ExtractStart(_chargrid, '^');
        _trail.Add(start);
        var current = start;


        while (CheckLimits(current.X, _xLen) && CheckLimits(current.Y, _yLen))
        {

            //p2
            var looping = _trail.Any(c => c.X == current.X && c.Y == current.Y && c.VisitedWhenWalking == direction && c != current);
            if (looping)
            {
                CircularExit = true;
                return _trail;
            }

            switch (direction)
            {
                case DirectionEnum.North:
                    if (!CheckLimits(current.Y - 1, _yLen))
                        return _trail;
                    if (_chargrid[current.Y - 1, current.X] == '#')
                        direction = Turn(direction);
                    else
                    {
                        current = new Coordinate(current.X, current.Y - 1) { VisitedWhenWalking = direction };
                        AddToTrail(current);
                    }
                    break;
                case DirectionEnum.East:
                    if (!CheckLimits(current.X + 1, _xLen))
                        return _trail;
                    if (_chargrid[current.Y, current.X + 1] == '#')
                        direction = Turn(direction);
                    else
                    {
                        current = new Coordinate(current.X + 1, current.Y) { VisitedWhenWalking = direction };
                        AddToTrail(current);

                    }
                    break;
                case DirectionEnum.South:
                    if (!CheckLimits(current.Y + 1, _yLen))
                        return _trail;
                    if (_chargrid[current.Y + 1, current.X] == '#')
                        direction = Turn(direction);
                    else
                    {
                        current = new Coordinate(current.X, current.Y + 1) { VisitedWhenWalking = direction };
                        AddToTrail(current);

                    }
                    break;
                case DirectionEnum.West:
                    if (!CheckLimits(current.X - 1, _xLen))
                        return _trail;
                    if (_chargrid[current.Y, current.X - 1] == '#')
                        direction = Turn(direction);
                    else
                    {
                        current = new Coordinate(current.X - 1, current.Y) { VisitedWhenWalking = direction };
                        AddToTrail(current);

                    }
                    break;
            }
        }

        //Should never end up here.
        return null;
    }

    void AddToTrail(Coordinate coordinate)
    {
        if (!_trail.Any(c => c.X == coordinate.X && c.Y == coordinate.Y))
            _trail.Add(coordinate);
    }

    private DirectionEnum Turn(DirectionEnum direction)
    {
        switch (direction)
        {
            case DirectionEnum.North:
                return DirectionEnum.East;
            case DirectionEnum.East:
                return DirectionEnum.South;
            case DirectionEnum.South:
                return DirectionEnum.West;
            case DirectionEnum.West:
                return DirectionEnum.North;

        }
        throw new NotImplementedException();
    }


    private Coordinate ExtractStart(char[,] grid, char startChar)
    {

        var ylen = grid.GetLength(0);
        var xlen = grid.GetLength(1);

        for (int i = 0; i < ylen; i++)
        {
            for (int j = 0; j < xlen; j++)
            {
                if (grid[i, j] == startChar)
                    return new Coordinate(j, i) { VisitedWhenWalking = DirectionEnum.North };
            }
        }

        throw new Exception("Missing start");
    }



    private bool CheckEast(int x, int y, string word)
    {
        //The entire word has been consumed in the north direction
        if (word == "")
            return true;
        //Check that we are on the map
        if (!CheckLimits(x, _xLen))
            return false;
        //If the first character of the remaining word is correct then we search further
        if (_chargrid[y, x] == word[0])
            return CheckEast(x + 1, y, word.Substring(1));
        return false;

    }

    private bool CheckSouth(int x, int y, string word)
    {
        //The entire word has been consumed in the north direction
        if (word == "")
            return true;
        //Check that we are on the map
        if (!CheckLimits(y, _yLen))
            return false;
        //If the first character of the remaining word is correct then we search further
        if (_chargrid[y, x] == word[0])
            return CheckSouth(x, y + 1, word.Substring(1));
        return false;

    }

    private bool CheckWest(int x, int y, string word)
    {
        //The entire word has been consumed in the north direction
        if (word == "")
            return true;
        //Check that we are on the map
        if (!CheckLimits(x, _xLen))
            return false;
        //If the first character of the remaining word is correct then we search further
        if (_chargrid[y, x] == word[0])
            return CheckWest(x - 1, y, word.Substring(1));
        return false;

    }
    private bool CheckLimits(int current, int limit)
    {
        if (current < 0)
            return false;
        if (current >= limit)
            return false;
        return true;
    }
}

public enum DirectionEnum
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}
