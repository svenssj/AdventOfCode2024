
using System.Buffers.Text;

public class Day5
{

    private List<string> ruleLines;
    private List<string> updateLines;

    private List<Rule>? rules;
    private List<Update>? updates;

    public Day5()
    {

        ruleLines = File.ReadAllLines("Day5\\inputrules.txt").ToList();
        updateLines = File.ReadAllLines("Day5\\inputupdates.txt").ToList();

    }

    public int Run()
    {

        rules = new List<Rule>();
        updates = new List<Update>();
        foreach (var line in ruleLines)
        {
            var splits = line.Split('|');

            rules.Add(new Rule()
            {
                Before = int.Parse(splits[0]),
                After = int.Parse(splits[1])
            });
        }

        rules = [.. rules.OrderBy(r => r.Before).ThenBy(r => r.After)];


        foreach (var line in updateLines)
        {

            updates.Add(new Update(line));
        }

        foreach (var update in updates)
        {
            update.CheckValid(rules);
        }

        var valid = updates.Where(u => u.Valid == true).ToList();

        var sum = valid.Sum(u => u.GetMiddle());
        //   Console.WriteLine("Valid middle page sum: " + sum);

        var invalid = updates.Where(u => u.Valid == false).ToList();

        foreach (var invalido in invalid)
        {

            invalido.Adjust(rules);

        }

        var middles = invalid.Select(i => i.GetMiddle());
        sum = middles.Sum();
        // Console.WriteLine("-------------Middles------------");
        // Console.WriteLine(string.Join(',', middles));
        //  Console.WriteLine("Invalido middle page sum: " + sum);


        return sum;

    }


}

public class Rule
{
    public required int Before { get; set; }
    public required int After { get; set; }
}

public class Update
{

    private readonly string original;
    public Update(string inputstring)
    {
        original = inputstring;
        Pages = inputstring.Split(',').Select(sp => int.Parse(sp)).ToList();
    }
    public bool Valid { get; set; }
    public List<int> Pages { get; set; }

    public void CheckValid(List<Rule> rules)
    {
        var relevantRules = rules.Where(r => Pages.Contains(r.Before) && Pages.Contains(r.After));
        foreach (var rule in relevantRules)
        {
            if (Pages.IndexOf(rule.After) < Pages.IndexOf(rule.Before))
            {
                Valid = false;
                return;
            }
        }
        Valid = true;
    }

    public void Adjust(List<Rule> rules)
    {
        var relevantRules = rules.Where(r => Pages.Contains(r.Before) && Pages.Contains(r.After)).ToList();
        while (!Valid)
        {
            foreach (var rule in relevantRules)
            {
                var afterindex = Pages.IndexOf(rule.After);
                var beforeindex = Pages.IndexOf(rule.Before);
                if (afterindex < beforeindex)
                {
                    Pages[afterindex] = rule.Before;
                    Pages[beforeindex] = rule.After;

                }

            }
            CheckValid(relevantRules);
        }
    }

    public void Adjust2(List<Rule> rules)
    {
        if (Valid)
            return;
        var relevantRules = rules.Where(r => Pages.Contains(r.Before) && Pages.Contains(r.After)).ToList();

        // Console.WriteLine("Invalid: " + original);
        // Console.WriteLine("-----------------------");
        // Console.WriteLine("Rules: ");
        // Console.WriteLine("-------------------------");
        foreach (var rule in relevantRules)
        {
         //   Console.WriteLine(rule.Before + "|" + rule.After);
        }
       // Console.WriteLine("-------------------------");

        Dictionary<int, int> NoOfPagesBeforePage = new Dictionary<int, int>();

        foreach (var page in Pages)
        {
            var pageIsBeforeNPages = relevantRules.Count(r => r.Before == page);
            NoOfPagesBeforePage.Add(page, pageIsBeforeNPages);
        }

        var tmp = NoOfPagesBeforePage.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key);

        Pages = tmp.ToList();

        CheckValid(relevantRules);

     //   Console.WriteLine(string.Join(',', Pages));
        // while (!Valid)
        // {
        //     foreach (var i in Pages)
        //     {
        //         var rulesWhereBefore = relevantRules.Where(r => r.Before == i).ToList();
        //         var rulesWhereAfter = relevantRules.Where(r => r.After == i).ToList();

        //         if (rulesWhereBefore.Count == Pages.Count - 1)
        //         //this needs to be first
        //         {
        //             Console.WriteLine("First");
        //         }

        //     }

        //     CheckValid(rules);
        // }


    }

    public void Print()
    {
        var s = string.Join(',', Pages);
        Console.WriteLine(original + " -> " + s);
    }

    public int GetMiddle()
    {


        var len = Pages.Count();
        if (len % 2 == 0)
            throw new Exception("Even no of pages");

        var floored = int.Parse(Math.Floor(len / 2.0).ToString());

        return Pages[floored];

    }
}