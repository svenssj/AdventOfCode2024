public class Day1
{

    public Day1()
    {

    }

    public void Run()
    {

        var list1 = new List<int>();
        var list2 = new List<int>();

        var lines = File.ReadAllLines("Day1\\input.txt");

        foreach (var line in lines)
        {
            var splits = line.Split("   ");

            list1.Add(int.Parse(splits[0]));
            list2.Add(int.Parse(splits[1]));
        }
        Part2(list1, list2);
    }

    public void Part2(List<int> list1, List<int> list2)
    {
        long score = 0;

        foreach(var i in list1)
        {
            var count = list2.Count(x => x == i);
            score = score + (i * count);
        }
        Console.WriteLine(score);
    }


    public void Part1(List<int> list1, List<int> list2)
    {
        
        list1.Sort();
        list2.Sort();

        var len = list1.Count();

        var diffs = new List<int>();

        for (int i = 0; i < len; i++)
        {
            var diff = list1[i] - list2[i];
            if (diff < 0)
                diff = diff * -1;
            diffs.Add(diff);
        }

        var sum = diffs.Sum();
        Console.WriteLine(sum);
    }

}