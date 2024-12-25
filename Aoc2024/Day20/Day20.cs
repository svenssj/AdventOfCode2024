using Microsoft.VisualBasic;

public class Day20
{
    Dictionary<string, int> _cheats;
    Location[,] _grid;
    List<Location> _flatList;
    DirectionEnum _direction;

    List<Coordinate> _possibleCheats;
    int _yLen;
    int _xLen;

    int CHEAT_LIM = 21;
    public Day20()
    {

        var lines = File.ReadAllLines("Day20\\input.txt");

        _yLen = lines.Count();
        _xLen = lines[0].Length;
        _cheats = [];
        _possibleCheats = [];
        _grid = new Location[_yLen, _xLen];
        _flatList = [];

        for (int i = 0; i < _yLen; i++)
            for (int j = 0; j < _xLen; j++)
            {
                var loc =
                new Location()
                {
                    X = j,
                    Y = i,
                    Value = lines[i][j]
                };
                _grid[i, j] = loc;
                _flatList.Add(loc);
            }

        Print();
    }

    public void Run()
    {

        var _start = _flatList.Single(l => l.Value == 'S');

        PrintRhombus(_start, true);
        var _end = _flatList.Single(l => l.Value == 'E');
        var current = _start;
        _start.LowestCostToGetHere = 0;
        _start.Visited = true;
        _start.WalkedHereFacing = _direction;
        while (!(current.X == _end.X && current.Y == _end.Y))
        {
            var left = GetLeft(current);
            if (left != null)
            {

                if (!left.LowestCostToGetHere.HasValue || current.LowestCostToGetHere + 1 < left.LowestCostToGetHere)
                {
                    left.LowestCostToGetHere = current.LowestCostToGetHere + 1;
                    left.BestWayHere = current;

                }
            }
            var forward = GetForward(current);
            if (forward != null)
                if (forward != null)
                {

                    if (!forward.LowestCostToGetHere.HasValue || current.LowestCostToGetHere + 1 < forward.LowestCostToGetHere)
                    {
                        forward.LowestCostToGetHere = current.LowestCostToGetHere + 1;
                        forward.BestWayHere = current;
                    }
                }
            var backwards = GetBackwards(current);
            if (backwards != null)
                if (backwards != null)
                {

                    if (!backwards.LowestCostToGetHere.HasValue || current.LowestCostToGetHere + 1 < backwards.LowestCostToGetHere)
                    {
                        backwards.LowestCostToGetHere = current.LowestCostToGetHere + 1;
                        backwards.BestWayHere = current;
                    }
                }
            var right = GetRight(current);
            if (right != null)
            {

                if (!right.LowestCostToGetHere.HasValue || current.LowestCostToGetHere + 1 < right.LowestCostToGetHere)
                {
                    right.LowestCostToGetHere = current.LowestCostToGetHere + 1;
                    right.BestWayHere = current;

                }
            }
            var tmp = current;
            current = _flatList.Where(l => l.Visited == false && l.LowestCostToGetHere.HasValue && l.Value == '.').OrderBy(l => l.LowestCostToGetHere).FirstOrDefault();
            if (current == null)
            {


                break;
            }
            current.Visited = true;
        }


        var path = _flatList.Where(v => v.Visited).OrderBy(l => l.LowestCostToGetHere).ToList();

        Print();
        path.ForEach(p =>
            p.Visited = false
        );

        foreach (var location in path)
        {
            location.Visited = true;
            // CheatPart1(location);
            CheatPart2(location);
        }


        Console.WriteLine(_flatList.Count(l => l.Visited));


        Console.WriteLine(_cheats.Count(kvp => kvp.Value >= 100));
    }

    private void CheatPart2(Location location)
    {


        var y = location.Y;
        var x = location.X;


        var nodes = GetCheatNodes(y, x, CHEAT_LIM);

        //neighbouring walls
        var ends = nodes.Where(n => n.Value !='#' && n.Visited == false);

        foreach (var end in ends)
        {
            var key = $"{x}-{y}|{end.X}-{end.Y}";
            if (!_cheats.ContainsKey(key))
                _cheats.Add(key, end.LowestCostToGetHere.Value - location.LowestCostToGetHere.Value - GetDistance(end, location));
        }

        //     var validEnds = nodes.Where(n => n.Value == '.' && !n.Visited);

        //     foreach (var end in validEnds)
        //     {
        //         var key = $"{south.X}-{south.Y}|{end.X}-{end.Y}";
        //         if (!_cheats.ContainsKey(key))
        //             _cheats.Add(key, end.LowestCostToGetHere.Value - location.LowestCostToGetHere.Value - GetDistance(end, location));

        // var south = GetLocation(y + 1, x, true);
        // if (south.Value == '#')
        // {
        //     var nodes = GetCheatNodes(south.Y, south.X);

        //     var validEnds = nodes.Where(n => n.Value == '.' && !n.Visited);

        //     foreach (var end in validEnds)
        //     {
        //         var key = $"{south.X}-{south.Y}|{end.X}-{end.Y}";
        //         if (!_cheats.ContainsKey(key))
        //             _cheats.Add(key, end.LowestCostToGetHere.Value - location.LowestCostToGetHere.Value - GetDistance(end, location));

        //     }

        // }
        // var north = GetLocation(y - 1, x, true);
        // if (north.Value == '#')
        // {
        //     var nodes = GetCheatNodes(north.Y, north.X);

        //     var validEnds = nodes.Where(n => n.Value == '.' && !n.Visited);

        //     foreach (var end in validEnds)
        //     {
        //         var key = $"{north.X}-{north.Y}|{end.X}-{end.Y}";
        //         if (!_cheats.ContainsKey(key))
        //             _cheats.Add(key, end.LowestCostToGetHere.Value - location.LowestCostToGetHere.Value - GetDistance(end, location));
        //     }
        // }
        // var west = GetLocation(y, x - 1, true);
        // if (west.Value == '#')
        // {
        //     var nodes = GetCheatNodes(west.Y, west.X);
        //     var validEnds = nodes.Where(n => n.Value == '.' && !n.Visited);

        //     foreach (var end in validEnds)
        //     {
        //         var key = $"{west.X}-{west.Y}|{end.X}-{end.Y}";
        //         if (!_cheats.ContainsKey(key))
        //             _cheats.Add($"{west.X}-{west.Y}|{end.X}-{end.Y}", end.LowestCostToGetHere.Value - location.LowestCostToGetHere.Value - GetDistance(end, location));
        //     }
        // }
        // var east = GetLocation(y, x + 1, true);
        // if (east.Value == '#')
        // {
        //     //  PrintRhombus(east, true);
        //     var nodes = GetCheatNodes(east.Y, east.X);
        //     var validEnds = nodes.Where(n => n.Value == '.' && !n.Visited);

        //     foreach (var end in validEnds)
        //     {
        //         var key = $"{east.X}-{east.Y}|{end.X}-{end.Y}";
        //         if (!_cheats.ContainsKey(key))
        //             _cheats.Add(key, end.LowestCostToGetHere.Value - location.LowestCostToGetHere.Value - GetDistance(end, location));
        //     }
        // }



    }

    private int GetDistance(Location from, Location to)
    {
        var xDiff = to.X - from.X;
        if (xDiff < 0)
            xDiff *= -1;
        var yDiff = to.Y - from.Y;
        if (yDiff < 0)
            yDiff *= -1;

        return xDiff + yDiff;
    }
    private void CheatPart1(Location location)
    {

        var y = location.Y;
        var x = location.X;
        var twoSouth = GetLocation(y + 2, x);
        if (twoSouth != null)
            if (!twoSouth.Visited) //Shortcut
            {
                var wall = GetLocation(y + 1, x);
                if (wall == null)
                {
                    _possibleCheats.Add(new Coordinate(x, y + 1));
                    _cheats.Add($"{x}-{y + 1}|{x}-{y + 2}", twoSouth.LowestCostToGetHere.Value - location.LowestCostToGetHere.Value - 2);
                }
            }
        var twoNorth = GetLocation(y - 2, x);
        if (twoNorth != null)
            if (!twoNorth.Visited) //Shortcut
            {
                var wall = GetLocation(y - 1, x);
                if (wall == null)
                {
                    _possibleCheats.Add(new Coordinate(x, y - 1));
                    _cheats.Add($"{x}-{y - 1}|{x}-{y - 2}", twoNorth.LowestCostToGetHere.Value - location.LowestCostToGetHere.Value - 2);
                }
            }
        var twoEast = GetLocation(y, x + 2);
        if (twoEast != null)
            if (!twoEast.Visited) //Shortcut
            {
                var wall = GetLocation(y, x + 1);
                if (wall == null)
                {
                    _possibleCheats.Add(new Coordinate(x + 1, y));
                    _cheats.Add($"{x + 1}-{y}|{x + 2}-{y}", twoEast.LowestCostToGetHere.Value - location.LowestCostToGetHere.Value - 2);
                }
            }
        var twoWest = GetLocation(y, x - 2);
        if (twoWest != null)
            if (!twoWest.Visited) //Shortcut
            {
                var wall = GetLocation(y, x - 1);
                if (wall == null)
                {
                    _possibleCheats.Add(new Coordinate(x + 1, y));
                    _cheats.Add($"{x - 1}-{y}|{x - 2}-{y}", twoWest.LowestCostToGetHere.Value - location.LowestCostToGetHere.Value - 2);
                }
            }
    }

    private List<Location> GetCheatNodes(int y, int x, int lim)
    {
        var list = new List<Location>();

        for (int i = -lim + 1; i < lim; i++)
        {
            var diff = i < 0 ? lim - (i * -1) : lim - i;
            if (diff < 0)
                diff *= -1;


            if (i != 0)
                list.Add(GetLocation(y + i, x, true));

            for (int j = 0; j < diff; j++)
            {
                list.Add(GetLocation(y + i, x - j, true));
                list.Add(GetLocation(y + i, x + j, true));
            }



        }
        return list.Where(l => l != null).ToList();


    }

    void PrintRhombus(Location start, bool highlight)
    {

        var nodes = GetCheatNodes(start.Y, start.X, 7);
        Console.WriteLine();
        for (int i = 0; i < _yLen; i++)
        {
            var s = "";
            for (int j = 0; j < _xLen; j++)
            {
                if (highlight)
                {
                    if (nodes.Contains(_grid[i, j]) && _grid[i, j].Value == '.')
                        s += 'O';
                    else
                        s += _grid[i, j].Value;
                }
                else
                {
                    if (nodes.Contains(_grid[i, j]))
                        s += 'X';
                    else
                        s += _grid[i, j].Value;
                }
            }
            Console.WriteLine(s);

        }
        Console.WriteLine();
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
    private Location? GetBackwards(Location l)
    {
        switch (_direction)
        {
            case DirectionEnum.South:
                return GetLocation(l.Y - 1, l.X);
            case DirectionEnum.West:
                return GetLocation(l.Y, l.X + 1);

            case DirectionEnum.North:
                return GetLocation(l.Y + 1, l.X);
            case DirectionEnum.East:
                return GetLocation(l.Y, l.X - 1);
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

    private Location? GetLocation(int y, int x, bool includeWall = false)
    {
        if (!CheckLimits(y, x))
            return null;

        var loc = _grid[y, x];
        if (includeWall)
            return loc;
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

    public void Print()
    {
        var y = _grid.GetLength(0);
        var x = _grid.GetLength(1);

        for (int i = 0; i < y; i++)
        {
            var s = "";
            for (int j = 0; j < x; j++)
                s += _grid[i, j].Visited ? 'O' : _grid[i, j].Value;
            Console.WriteLine(s);
        }
    }
}