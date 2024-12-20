using System.Text;

public class Day12
{



    List<Region> Regions;
    Plot[,] Grid;
    int ylen;
    int xlen;
    public Day12()
    {
        var lines = File.ReadAllLines("Day12\\input.txt");

        ylen = lines.Length;
        xlen = lines[0].Length;

        Grid = new Plot[ylen, xlen];
        Regions = new List<Region>();
        for (int i = 0; i < ylen; i++)
        {
            for (int j = 0; j < xlen; j++)
            {
                Grid[i, j] = new Plot()
                {
                    Type = lines[i][j],
                    Y = i,
                    X = j,

                };
            }
        }

        Print();

    }



    public void Run()
    {

        Plot FirstNotCounted;

        while (true)
        {
            FirstNotCounted = GetFirstNotCounted();
            if (FirstNotCounted == null)
                break;
            var plots = GetSameTypeNeighbours(FirstNotCounted);

            Regions.Add(new Region { Type = plots[0].Type, Plots = plots });
        }

        var price = 0;

        foreach (var region in Regions)
        {
            Console.WriteLine(region.Type);
            var area = GetArea(region);
            var sides = CountSides(region);
            Console.WriteLine($"{area} * {sides} = {area * sides}");
            price += area * sides;
        }


        Console.WriteLine(price);
    }

    private Plot? GetFirstNotCounted()
    {
        for (int i = 0; i < ylen; i++)
        {

            for (int j = 0; j < xlen; j++)
            {
                if (Grid[i, j].Counted == false)
                    return Grid[i, j];
            }

        }
        return null;
    }

    private int GetPerimeter(Region region)
    {
        var count = 0;
        foreach (var plot in region.Plots)
        {
            count += 4 - plot.SameTypeNeighbours;
        }
        return count;
    }

    private int CountSides(Region region)
    {
        if (region.Plots.Count == 1)
            return 4;
        if (region.Plots.Count == 2)
            return 4;


        var sides = 0;

        region.Plots.Select(p => p.Counted == false);


        foreach (var plot in region.Plots)
        {
            var corners = GetCornerCount(plot);
            if (corners > 0)
            {
                Console.WriteLine($"found {corners} at [{plot.Y},{plot.X}]");
            }
            sides += corners;
        }

        return sides;
    }

    private int GetArea(Region region)
    {
        return region.Plots.Count;
    }

    private List<Plot> GetSameTypeNeighbours(Plot location)
    {
        List<Plot> plotsInRegion = [];
        location.Counted = true;
        plotsInRegion.Add(location);
        var count = 0;

        var north = GetCoordinate(location.X, location.Y - 1);
        if (north != null && north.Type == location.Type)
        {

            count++;
            if (!north.Counted)
                plotsInRegion.AddRange(GetSameTypeNeighbours(north));
        }

        var east = GetCoordinate(location.X + 1, location.Y);
        if (east != null && east.Type == location.Type)
        {

            count++;
            if (!east.Counted)
                plotsInRegion.AddRange(GetSameTypeNeighbours(east));
        }

        var west = GetCoordinate(location.X - 1, location.Y);
        if (west != null && west.Type == location.Type)
        {

            count++;
            if (!west.Counted)
                plotsInRegion.AddRange(GetSameTypeNeighbours(west));
        }

        var south = GetCoordinate(location.X, location.Y + 1);
        if (south != null && south.Type == location.Type)
        {

            count++;
            if (!south.Counted)
                plotsInRegion.AddRange(GetSameTypeNeighbours(south));
        }
        location.SameTypeNeighbours = count;

        return plotsInRegion;

    }

    private Plot? GetCoordinate(int x, int y)
    {
        if (CheckLimits(y, x))
            return Grid[y, x];
        return null;

    }

    private bool CheckLimits(int y, int x)
    {
        if (y < 0)
            return false;
        if (y >= ylen)
            return false;
        if (x < 0)
            return false;
        if (x >= xlen)
            return false;
        return true;
    }

    public void Print()
    {

        for (int i = 0; i < ylen; i++)
        {
            var sb = new StringBuilder();
            for (int j = 0; j < xlen; j++)
            {
                sb.Append(Grid[i, j].Type);
            }
            Console.WriteLine(sb.ToString());
        }
    }

    public int GetCornerCount(Plot plot)
    {


        var t = plot.Type;
        var n = GetNorth(plot);
        var s = GetSouth(plot);
        var w = GetWest(plot);
        var e = GetEast(plot);
        var nw = GetNorthWest(plot);
        var ne = GetNorthEast(plot);
        var sw = GetSouthWest(plot);
        var se = GetSouthEast(plot);

        // if (plot.Y == 0 && plot.X == 2)
        // {
        //     Console.WriteLine("--------------");
        //     Console.WriteLine(nw.ToString() + n + ne);
        //     Console.WriteLine(w.ToString() + t + e);
        //     Console.WriteLine(sw.ToString() + s + se);
        //     Console.WriteLine("--------------");
        // }






        //SINGLE

        if (n != t && s != t && w != t && e != t && nw != t && sw != t && se != t && ne != t)
            return 4;

        var corners = 0;
        //OUTER

        //XBX
        //BAX
        //XXX
        if (w != t && n != t)
            corners++;
        //XBX
        //XAB
        //XXX
        if (e != t && n != t)
            corners++;
        //XXX
        //XAB
        //XBX
        if (e != t && s != t)
            corners++;
        //XXX
        //BAX
        //XBX
        if (w != t && s != t)
            corners++;


        ///INNER



        //BAX
        //AAX
        //XXX
        if (w == t && n == t && nw != t)
            corners++;
        //XAB
        //XAA
        //XXX
        if (e == t && n == t && ne != t)
            corners++;
        //XXX
        //XAA
        //XAB
        if (e == t && s == t && se != t)
            corners++;
        //XXX
        //AAX
        //BAX
        if (w == t && s == t && sw != t)
            corners++;

        return corners;

    }
    public char GetNorth(Plot plot)
    {
        if (CheckLimits(plot.Y - 1, plot.X))
            return Grid[plot.Y - 1, plot.X].Type;
        return '#';
    }
    public char GetSouth(Plot plot)
    {

        if (CheckLimits(plot.Y + 1, plot.X))
            return Grid[plot.Y + 1, plot.X].Type;
        return '#';

    }
    public char GetWest(Plot plot)
    {
        if (CheckLimits(plot.Y, plot.X - 1))
            return Grid[plot.Y, plot.X - 1].Type;
        return '#';
    }
    public char GetEast(Plot plot)
    {
        if (CheckLimits(plot.Y, plot.X + 1))
            return Grid[plot.Y, plot.X + 1].Type;
        return '#';
    }

    public char GetNorthEast(Plot plot)
    {
        if (CheckLimits(plot.Y - 1, plot.X + 1))
            return Grid[plot.Y - 1, plot.X + 1].Type;
        return '#';
    }
    public char GetSouthEast(Plot plot)
    {
        if (CheckLimits(plot.Y + 1, plot.X + 1))
            return Grid[plot.Y + 1, plot.X + 1].Type;
        return '#';
    }
    public char GetSouthWest(Plot plot)
    {
        if (CheckLimits(plot.Y + 1, plot.X - 1))
            return Grid[plot.Y + 1, plot.X - 1].Type;
        return '#';
    }
    public char GetNorthWest(Plot plot)
    {
        if (CheckLimits(plot.Y - 1, plot.X - 1))
            return Grid[plot.Y - 1, plot.X - 1].Type;
        return '#';
    }


}

public class Plot
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool Counted { get; set; }

    public int SameTypeNeighbours { get; set; }
    public char Type { get; set; }
}
public class Region
{
    public char Type { get; set; }
    public bool Counted { get; set; }
    public List<Plot> Plots { get; set; }
}
