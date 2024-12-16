using System.Drawing;
using System.Reflection;

public class Day8
{

    private int _xLen;
    private int _yLen;
    private readonly char[,] _chargrid;

    private HashSet<char> _frequencies;

    public Day8()
    {
        var lines = File.ReadAllLines("Day8\\input.txt");

        _xLen = lines[0].Length;
        _yLen = lines.Length;
        _frequencies = new HashSet<char>();

        _chargrid = new char[_yLen, _xLen];

        for (int i = 0; i < _yLen; i++)
        {
            for (int j = 0; j < _xLen; j++)
            {
                _chargrid[i, j] = lines[i][j];
                _frequencies.Add(lines[i][j]);
            }
        }
        _frequencies.RemoveWhere(x => x == '.');
    }

    public void Run()
    {
        var antinodes = new HashSet<Tuple<int, int>>();

        foreach (var frequency in _frequencies)
        {
            var locations = ExtractStarts(_chargrid, frequency);
            var count = locations.Count;
            for (int i = 0; i < count; i++)
            {

                var current = locations[i];
                if (count >= 2)
                {
                    antinodes.Add(new Tuple<int, int>(current.Y, current.X));
                }
                for (int j = 0; j < count; j++)
                {


                    if (i == j)
                        continue;


                    var other = locations[j];
                    var yDiff = other.Y - current.Y;
                    var xDiff = other.X - current.X;
                    int mul = 1;
                    while (CheckLimits(other.Y + (yDiff * mul), other.X + (xDiff * mul)))
                    {
                        antinodes.Add(new Tuple<int, int>(other.Y + (yDiff * mul), other.X + (xDiff * mul)));
                        mul++;
                    }

                }
            }
        }

        var withinbounds = antinodes.Where(t => CheckLimits(t.Item1, t.Item2));
        Console.WriteLine(withinbounds.Count());
    }

    private List<Coordinate> ExtractStarts(char[,] grid, char startChar)
    {

        var startingCoordinates = new List<Coordinate>();
        var ylen = grid.GetLength(0);
        var xlen = grid.GetLength(1);

        for (int i = 0; i < ylen; i++)
        {
            for (int j = 0; j < xlen; j++)
            {
                if (grid[i, j] == startChar)
                    startingCoordinates.Add(new Coordinate(j, i));

            }
        }

        return startingCoordinates;
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