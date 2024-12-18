using System.Text;
using Microsoft.VisualBasic;

public class Day16
{

    private int _xLen;
    private int _yLen;
    public Location[,] _grid;
    public List<Location> _flatList;
    private Location _start;
    private Location _end;

    public DirectionEnum _direction;

    public Day16()
    {


        var lines = File.ReadAllLines("Day16\\eight.txt");
        _flatList = new List<Location>();
        _yLen = lines.Length;
        _xLen = lines[0].Length;

        _grid = new Location[_yLen, _xLen];
        for (int i = 0; i < _yLen; i++)
        {
            for (int j = 0; j < _xLen; j++)
            {
                var tmp = new Location()
                {
                    Value = lines[i][j],
                    X = j,
                    Y = i
                };
                _flatList.Add(tmp);
                if (tmp.Value == 'S')
                    _start = tmp;
                else if (tmp.Value == 'E')
                    _end = tmp;
                _grid[i, j] = tmp;
            }
        }
        _direction = DirectionEnum.East;
        //  Print();

    }

    public void Run()
    {
        var current = _start;
        _start.LowestCostToGetHere = 0;
        _start.Visited = true;
        _start.WalkedHereFacing = _direction;
        while (!(current.X == _end.X && current.Y == _end.Y))
        {
            _direction = current.WalkedHereFacing;
            var left = GetLeft(current);
            if (left != null)
            {
                var leftSteps = current.LowestCostToGetHere.Value + 1 + 1000;
                left.PossibleWaysToGetHere.Add(current);
                if (!left.LowestCostToGetHere.HasValue || leftSteps < left.LowestCostToGetHere)
                {

                    left.LowestCostToGetHere = leftSteps;
                    left.BestWayHere = current;
                    left.WalkedHereFacing = TurnLeft(_direction);
                }

            }
            var forward = GetForward(current);
            if (forward != null)
                if (forward != null)
                {
                    var forwardSteps = current.LowestCostToGetHere.Value + 1;

                    forward.PossibleWaysToGetHere.Add(current);
                    if (!forward.LowestCostToGetHere.HasValue || forwardSteps < forward.LowestCostToGetHere)
                    {

                        forward.LowestCostToGetHere = forwardSteps;
                        forward.BestWayHere = current;
                        forward.WalkedHereFacing = _direction;
                    }

                }
            var right = GetRight(current);
            if (right != null)
            {
                var rightSteps = current.LowestCostToGetHere.Value + 1 + 1000;

                right.PossibleWaysToGetHere.Add(current);
                if (!right.LowestCostToGetHere.HasValue || rightSteps < right.LowestCostToGetHere)
                {

                    right.LowestCostToGetHere = rightSteps;
                    right.BestWayHere = current;
                    right.WalkedHereFacing = TurnRight(_direction);
                }

            }
            var tmp = current;
            current = _flatList.Where(l => l.Visited == false && l.LowestCostToGetHere.HasValue).OrderBy(l => l.LowestCostToGetHere).FirstOrDefault();
            if (current == null)
                break;
            current.Visited = true;
            //  Thread.Sleep(50);
            // Console.WriteLine();
            // Print(true);
        }

        //Walk best path in reverse add trails
        var uniqueLocations = WalkBack(_end, _start);
        Print(false);
        Print2();
        Console.WriteLine();
        Console.WriteLine("Lowest cost to end: " + _end.LowestCostToGetHere);
        Console.WriteLine(uniqueLocations.Count);
    }

    private HashSet<Location> WalkBack(Location beginning, Location target)
    {
        var locations = new HashSet<Location>();
        locations.Add(beginning);
        locations.Add(target);
        var current = beginning;
        while (!(current.X == target.X && current.Y == target.Y))
        {
            current.PrintedBackwards = true;
            current = current.BestWayHere;
            locations.Add(current);

            var ways = current.PossibleWaysToGetHere;
            if (ways.Count > 1)
                foreach (var way in ways)
                {
                    if (!locations.Contains(way))
                        locations.UnionWith(WalkBack(way, target));
                }


        }
        target.PrintedBackwards = true;
        return locations;
    }

    private void Print(bool console)
    {

        var p = "Day16\\output.txt";
        if (File.Exists(p))
            File.Delete(p);
        var lines = new List<string>();
        for (int i = 0; i < _yLen; i++)
        {
            var s = "";
            for (int j = 0; j < _xLen; j++)
            {
                s += _grid[i, j].Visited ? "X" : _grid[i, j].Value;
            }
            if (console)
                Console.WriteLine(s);
            lines.Add(s);
        }
        File.WriteAllLines(p, lines);
    }

    private void Print2()
    {
        var p = "Day16\\output2.txt";
        if (File.Exists(p))
            File.Delete(p);
        var lines = new List<string>();
        for (int i = 0; i < _yLen; i++)
        {
            var s = "";
            for (int j = 0; j < _xLen; j++)
            {
                s += _grid[i, j].PrintedBackwards ? 'O' : _grid[i, j].Visited ? "." : _grid[i, j].Value;
            }
            lines.Add(s);
        }
        File.WriteAllLines(p, lines);
    }
    private Location? GetForward(Location l)
    {
        switch (_direction)
        {
            case DirectionEnum.South:
                return GetLocation(l.Y + 1, l.X);
            case DirectionEnum.West:
                return GetLocation(l.Y, l.X - 1);

            case DirectionEnum.North:
                return GetLocation(l.Y - 1, l.X);
            case DirectionEnum.East:
                return GetLocation(l.Y, l.X + 1);
        }
        return null;
    }
    private Location? GetLeft(Location l)
    {
        switch (_direction)
        {
            case DirectionEnum.South:
                return GetLocation(l.Y, l.X + 1);

            case DirectionEnum.West:
                return GetLocation(l.Y + 1, l.X);


            case DirectionEnum.North:
                return GetLocation(l.Y, l.X - 1);

            case DirectionEnum.East:
                return GetLocation(l.Y - 1, l.X);
        }
        return null;
    }
    private Location? GetRight(Location l)
    {
        switch (_direction)
        {
            case DirectionEnum.South:
                return GetLocation(l.Y, l.X - 1);

            case DirectionEnum.West:
                return GetLocation(l.Y - 1, l.X);
            case DirectionEnum.North:
                return GetLocation(l.Y, l.X + 1);
            case DirectionEnum.East:
                return GetLocation(l.Y + 1, l.X);
        }
        return null;
    }

    private DirectionEnum TurnRight(DirectionEnum direction)
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
    private DirectionEnum TurnLeft(DirectionEnum direction)
    {
        switch (direction)
        {
            case DirectionEnum.North:
                return DirectionEnum.West;
            case DirectionEnum.East:
                return DirectionEnum.North;
            case DirectionEnum.South:
                return DirectionEnum.East;
            case DirectionEnum.West:
                return DirectionEnum.South;

        }
        throw new NotImplementedException();
    }
    private Location? GetLocation(int y, int x)
    {
        if (!CheckLimits(y, x))
            return null;

        var loc = _grid[y, x];
        if (loc.Value == '#')
            return null;
        return loc;
    }

    private bool CheckLimits(int y, int x)
    {
        if (y < 0)
            return false;
        if (y >= _yLen)
            return false;
        if (x < 0)
            return false;
        if (x >= _xLen)
            return false;
        return true;
    }
}

public class Location
{

    public Location()
    {
        PossibleWaysToGetHere = [];
    }
    public bool PrintedBackwards { get; set; }
    public bool Visited { get; set; }
    public int? LowestCostToGetHere { get; set; }
    public char Value { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public Location BestWayHere { get; set; }
    public List<Location> PossibleWaysToGetHere { get; set; }
    public DirectionEnum WalkedHereFacing { get; set; }
}