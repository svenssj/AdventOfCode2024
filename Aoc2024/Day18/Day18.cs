public class Day18
{

    Location[,] _grid;
    List<Coordinate> _bitsToCorrupt;
    List<Location> _flatList;
    DirectionEnum _direction;
    private int LIMIT;
    int _yLen;
    int _xLen;
    public Day18()
    {

        var lines = File.ReadAllLines("Day18\\input.txt");
        _bitsToCorrupt = [];
        foreach (var line in lines)
        {
            var splits = line.Split(',');
            _bitsToCorrupt.Add(new Coordinate(int.Parse(splits[0]), int.Parse(splits[1])));
        }

        LIMIT = 71;
        _yLen = LIMIT;
        _xLen = LIMIT;

        _grid = new Location[LIMIT, LIMIT];
        _flatList = [];

        for (int i = 0; i < LIMIT; i++)
            for (int j = 0; j < LIMIT; j++)
            {
                var loc =
                new Location()
                {
                    X = j,
                    Y = i,
                    Value = '.'
                };
                _grid[i, j] = loc;
                _flatList.Add(loc);

            }


    }

    public void Run()
    {

        //Corrupt

        var tot = _bitsToCorrupt.Count;
        var curr = 0;
        foreach (var bit in _bitsToCorrupt)
        {
            Console.WriteLine($"{curr}/{tot}");
            curr++;
            _grid[bit.Y, bit.X].Value = '#';
            //Reset path
            foreach (var location in _flatList)
            {
                location.Visited = false;
                location.BestWayHere = null;
                location.LowestCostToGetHere = null;

            }
            //   Print();

            var _start = _grid[0, 0];
            var _end = _grid[LIMIT - 1, LIMIT - 1];
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
                    WalkBack(tmp, _start);
                    Print();
                    break;
                }
                current.Visited = true;
            }
        }






    }

    private HashSet<Location> WalkBack(Location beginning, Location target)
    {
        var locations = new HashSet<Location>();

        locations.Add(target);
        var current = beginning;
        while (!(current.X == target.X && current.Y == target.Y))
        {
            current.PrintedBackwards = true;
            current = current.BestWayHere;
            locations.Add(current);

            // foreach (var way in current.PossibleWaysHere)
            // {
            //     if (!locations.Contains(way))
            //         locations.UnionWith(WalkBack(way, target));
            // }


        }
        target.PrintedBackwards = true;
        return locations;
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

    public void Print()
    {
        var y = _grid.GetLength(0);
        var x = _grid.GetLength(1);

        for (int i = 0; i < y; i++)
        {
            var s = "";
            for (int j = 0; j < x; j++)
                s += _grid[i, j].PrintedBackwards ? 'O' : _grid[i, j].Value;
            Console.WriteLine(s);
        }
    }
}