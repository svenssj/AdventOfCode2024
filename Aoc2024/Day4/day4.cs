using System.Drawing;
using System.Reflection;

public class Day4
{

    private int _xLen;
    private int _yLen;
    private readonly char[,] _chargrid;

    private readonly string _word;

    public Day4()
    {
        var lines = File.ReadAllLines("Day4\\example.txt");

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
        _word = "XMAS";
    }

    public void Run()
    {

        //p1
        var starts = ExtractStarts(_chargrid, _word[0]);
        var sum = 0;
        foreach (var start in starts)
        {

            var goesNorth = CheckNorth(start.X, start.Y, _word);
            var goesNorthEast = CheckNorthEast(start.X, start.Y, _word);
            var goesEast = CheckEast(start.X, start.Y, _word);
            var goesSouthEast = CheckSouthEast(start.X, start.Y, _word);
            var goesSouth = CheckSouth(start.X, start.Y, _word);
            var goesSouthWest = CheckSouthWest(start.X, start.Y, _word);
            var goesWest = CheckWest(start.X, start.Y, _word);
            var goesNorthWest = CheckNorthWest(start.X, start.Y, _word);


            var bools = new List<bool>(){
                goesNorth,
                goesNorthEast,
                goesEast ,
                goesSouthEast,
                goesSouth ,
                goesSouthWest,
                goesWest,
                goesNorthWest
            };

            var count = bools.Count(b => b == true);
            if(count==8)
                Console.WriteLine(start.X + "    " + start.Y);
            sum = sum + count;
        }

        Console.WriteLine(sum);



        starts = ExtractStarts(_chargrid, 'A');
        var AM = "AM";
        var AS = "AS";
        var findings = 0;
        foreach (var start in starts)
        {

            var score = 0;
            if (CheckNorthWest(start.X, start.Y, AM) && CheckSouthEast(start.X, start.Y, AS))
                score++;
            else if (CheckSouthEast(start.X, start.Y, AM) && CheckNorthWest(start.X, start.Y, AS))
                score++;
            if (CheckNorthEast(start.X, start.Y, AM) && CheckSouthWest(start.X, start.Y, AS))
                score++;
            else if (CheckSouthWest(start.X, start.Y, AM) && CheckNorthEast(start.X, start.Y, AS))
                score++;


            if (score == 2)
                findings++;
        }
        Console.WriteLine(findings);
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


    private bool CheckNorth(int x, int y, string word)
    {
        //The entire word has been consumed in the north direction
        if (word == "")
            return true;
        //Check that we are on the map
        if (!CheckLimits(y, _yLen))
            return false;
        //If the first character of the remaining word is correct then we search further
        if (_chargrid[y, x] == word[0])
            return CheckNorth(x, y - 1, word.Substring(1));
        return false;

    }
    private bool CheckNorthEast(int x, int y, string word)
    {
        //The entire word has been consumed in the north direction
        if (word == "")
            return true;
        //Check that we are on the map
        if (!CheckLimits(y, _yLen))
            return false;
        if (!CheckLimits(x, _xLen))
            return false;
        //If the first character of the remaining word is correct then we search further
        if (_chargrid[y, x] == word[0])
            return CheckNorthEast(x + 1, y - 1, word.Substring(1));
        return false;

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
    private bool CheckSouthEast(int x, int y, string word)
    {
        //The entire word has been consumed in the north direction
        if (word == "")
            return true;
        //Check that we are on the map
        if (!CheckLimits(y, _yLen))
            return false;
        if (!CheckLimits(x, _xLen))
            return false;
        //If the first character of the remaining word is correct then we search further
        if (_chargrid[y, x] == word[0])
            return CheckSouthEast(x + 1, y + 1, word.Substring(1));
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
    private bool CheckSouthWest(int x, int y, string word)
    {
        //The entire word has been consumed in the north direction
        if (word == "")
            return true;
        //Check that we are on the map
        if (!CheckLimits(y, _yLen))
            return false;
        if (!CheckLimits(x, _xLen))
            return false;
        //If the first character of the remaining word is correct then we search further
        if (_chargrid[y, x] == word[0])
            return CheckSouthWest(x - 1, y + 1, word.Substring(1));
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
    private bool CheckNorthWest(int x, int y, string word)
    {
        //The entire word has been consumed in the north direction
        if (word == "")
            return true;
        //Check that we are on the map
        if (!CheckLimits(x, _xLen))
            return false;
        if (!CheckLimits(y, _yLen))
            return false;
        //If the first character of the remaining word is correct then we search further
        if (_chargrid[y, x] == word[0])
            return CheckNorthWest(x - 1, y - 1, word.Substring(1));
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

public class Coordinate
{
    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Value  { get; set; }

    public DirectionEnum VisitedWhenWalking { get; set; }
}