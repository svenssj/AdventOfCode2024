public class Day2
{
    public Day2()
    {

    }

    public void Run()
    {
        var rows = File.ReadAllLines("Day2\\input.txt");

        List<List<int>> reports = [];

        foreach (var row in rows)
        {
            var stringLevels = row.Split(" ");

            var ints = stringLevels.Select(x => int.Parse(x));
            reports.Add(ints.ToList());
        }
        int safeCount = 0;
        foreach (var report in reports)
        {
            //We need to try both ways since it might change if we remove the first or second index with the Problem Dampener
            //But we dont need to try if its both increasing or decreasing.
            if (CheckLevelsSafe(report, VersionEnum.Increasing))
                safeCount++;
            else if (CheckLevelsSafe(report, VersionEnum.Decreasing))
                safeCount++;
        }
        Console.WriteLine(safeCount);
    }

    public bool CheckLevelsSafe(List<int> levels, VersionEnum version, bool pda = false)
    {
        var count = levels.Count() - 1; //-1 because we use i+1. 

        for (int i = 0; i < count; i++)
        {
            int a = levels[i];
            int b = levels[i + 1];
            if (!(CheckPairSafe(version, a, b)))
            {
                if (pda)
                {
                    //Problem dampener has been activated and we ran into another problem,
                    //The report is screwed,
                    return false;
                }
                else
                {
                    //Either i or i+1 has to be removed 
                    var l1 = new List<int>(levels);
                    l1.RemoveAt(i);
                    var l2 = new List<int>(levels);
                    l2.RemoveAt(i + 1);
                    //Checking if either works, 
                    return CheckLevelsSafe(l1, version, true) || CheckLevelsSafe(l2, version, true);
                }
            }

        }

        return true;
    }

    public bool CheckPairSafe(VersionEnum version, int a, int b)
    {
        if (version == VersionEnum.Increasing)
        {
            //a and b are equal
            //a is larger than b thus not increasing
            //the difference is larger than 3
            if (a == b || a > b || b - a > 3)
                return false;
            return true;
        }
        else
        {
            //a and b are equal
            //a is smaller than b thus not decreasing
            //the difference is larger than 3
            if (a == b || a < b || a - b > 3)
                return false;
            return true;
        }
    }
}

public enum VersionEnum
{
    Increasing,
    Decreasing
}