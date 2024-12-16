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


        var lines = File.ReadAllLines("Day16\\example.txt");
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
        Print();

    }

    public void Run()
    {
        var current = _start;
        _start.LowestCostToGetHere = 0;
        _start.Visited = true;

        while (current.X != _end.X && current.Y != _end.Y)
        {
            var left = GetLeft(current);
            if (left != null)
            {
                if (!left.LowestCostToGetHere.HasValue || current.LowestCostToGetHere + 1 + 1000 < left.LowestCostToGetHere)
                {
                    left.LowestCostToGetHere = current.LowestCostToGetHere + 1 + 1000;
                    left.ArrivedHereFrom = current;
                }
            }
            var forward = GetForward(current);
            if (forward != null)
                if (forward != null)
                {
                    if (!forward.LowestCostToGetHere.HasValue || current.LowestCostToGetHere + 1 < forward.LowestCostToGetHere)
                    {
                        forward.LowestCostToGetHere = current.LowestCostToGetHere + 1;
                        forward.ArrivedHereFrom = current;
                    }
                }
            var right = GetRight(current);
            if (right != null)
            {
                if (!right.LowestCostToGetHere.HasValue || current.LowestCostToGetHere + 1 + 1000 < right.LowestCostToGetHere)
                {
                    right.LowestCostToGetHere = current.LowestCostToGetHere + 1 + 1000;
                    right.ArrivedHereFrom = current;
                }
            }
            var tmp = current;
            current = _flatList.Where(l => l.Visited == false && l.LowestCostToGetHere.HasValue).OrderBy(l => l.LowestCostToGetHere).First();
            current.Visited = true;
            Print();
        }




    }

    private void Print()
    {

        for (int i = 0; i < _yLen; i++)
        {
            var s = "";
            for (int j = 0; j < _xLen; j++)
            {
                s += _grid[i, j].Visited ? "¤" : _grid[i, j].Value;
            }
            Console.WriteLine(s);
        }
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
    private List<Location> GetNeighbours(Location l)
    {
        var neighbours = new List<Location>();
        var north = GetLocation(l.Y - 1, l.X);
        if (north != null)
            neighbours.Add(north);

        var south = GetLocation(l.Y - 1, l.X);
        if (south != null)
            neighbours.Add(south);

        var west = GetLocation(l.Y, l.X - 1);
        if (west != null)
            neighbours.Add(west);

        var east = GetLocation(l.Y, l.X + 1);
        if (east != null)
            neighbours.Add(east);

        return neighbours;
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
    public bool Visited { get; set; }
    public int? LowestCostToGetHere { get; set; }
    public char Value { get; set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public Location ArrivedHereFrom { get; internal set; }
}