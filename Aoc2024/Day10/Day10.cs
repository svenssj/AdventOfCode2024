using Microsoft.VisualBasic;

public class Day10
{



    int[,] _grid;

    int _yLen;
    int _xLen;


    public Day10()
    {
        var lines = File.ReadAllLines("Day10\\input.txt");

        _yLen = lines.Count();
        _xLen = lines[0].Count();

        _grid = new int[_yLen, _xLen];

        for (int i = 0; i < _yLen; i++)
        {
            for (int j = 0; j < _xLen; j++)
            {
                _grid[i, j] = int.Parse(lines[i][j].ToString());
            }
        }
    }

    public void Run()
    {

        var starts = new List<Coordinate>();

        for (int i = 0; i < _yLen; i++)
        {
            for (int j = 0; j < _xLen; j++)
            {
                if (_grid[i, j] == 0)
                {
                    starts.Add(new Coordinate(j, i) { Value = 0 });
                }
            }
        }
        var scoreSum = 0;


        //p1
        foreach (var start in starts)
        {
            var score = Score(_grid, start);
            scoreSum += score;
            Console.WriteLine(score);

        }
        Console.WriteLine("SUM: " + scoreSum);



        var paths = new List<string>();
        //p2 
        foreach (var start in starts)
        {
            paths.AddRange(WalkPath(start, _grid));
        }

        

    }

    private int Score(int[,] grid, Coordinate trailHead)
    {
        List<Coordinate> locationsToVisit = [];
        locationsToVisit.Add(trailHead);
        var ends = new HashSet<Tuple<int, int>>();


        List<string> trails = [];
        trails.Add($"|{trailHead.X}+{trailHead.Y}");
        while (locationsToVisit.Any())
        {
            var instanceLocations = locationsToVisit;
            locationsToVisit = [];

            foreach (var location in instanceLocations)
            {
                var neighbours = GetValidNeighbours(location, grid);
                var stops = neighbours.Where(c => c.Value == 9);
                foreach (var stop in stops)
                {
                    ends.Add(new(stop.Y, stop.X));
                }
                var paths = neighbours.Where(c => c.Value < 9);

                locationsToVisit.AddRange(paths);
            }


        }
        return ends.Count();

    }

    private List<string> WalkPath(Coordinate toVisit, int[,] grid)
    {
        if (toVisit.Value == 9)
        {
            return new List<string>(){
                $"|{toVisit.X}+{toVisit.Y}"
            };
        }
        else
        {

            var neighbours = GetValidNeighbours(toVisit, grid);
            List<string> walkedPath = [];
            foreach (var neighbour in neighbours)
            {
                var trails = WalkPath(neighbour, grid);
                for (int i = 0; i < trails.Count; i++)
                {
                    trails[i] = $"|{toVisit.X}+{toVisit.Y}" + trails[i];
                }
                walkedPath.AddRange(trails);
            }
            return walkedPath;
        }
    }

    private List<Coordinate> GetValidNeighbours(Coordinate location, int[,] grid)
    {
        List<Coordinate> locationsToVisit = [];
        var oneStepUp = location.Value + 1;


        var north = GetCoordinate(location.X, location.Y - 1, grid);
        if (north != null && north.Value == oneStepUp)
            locationsToVisit.Add(north);

        var east = GetCoordinate(location.X + 1, location.Y, grid);
        if (east != null && east.Value == oneStepUp)
            locationsToVisit.Add(east);

        var west = GetCoordinate(location.X - 1, location.Y, grid);
        if (west != null && west.Value == oneStepUp)
            locationsToVisit.Add(west);

        var south = GetCoordinate(location.X, location.Y + 1, grid);
        if (south != null && south.Value == oneStepUp)
            locationsToVisit.Add(south);

        return locationsToVisit;

    }
    private Coordinate? GetCoordinate(int x, int y, int[,] grid)
    {

        if (CheckLimits(y, x))
            return new Coordinate(x, y) { Value = grid[y, x] };
        return null;

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


public class Trail
{

    public Coordinate TrailHead { get; set; }
    public List<string> Pahts { get; set; }
}