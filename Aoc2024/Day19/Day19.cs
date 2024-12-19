using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

public class Day19
{

    public HashSet<string> _towelPatterns;

    public List<TowelDesign> _designs;

    private Dictionary<string, long> _possiblecombinationscache;
    private Dictionary<string, bool> _cancreatecache;
    public Day19()
    {
        _towelPatterns = new HashSet<string>();
        _designs = [];
        _possiblecombinationscache = [];
        _cancreatecache = [];
        var lines = File.ReadAllLines("Day19\\input.txt");

        var availablePatterns = lines.Take(1).ToArray()[0];

        _towelPatterns = availablePatterns.Split(", ").ToHashSet();

        lines = lines.Skip(2).ToArray();

        foreach (var line in lines)
        {
            _designs.Add(new TowelDesign()
            {
                TargetPattern = line
            });
        }
    }

    public void Run()
    {
        var i = 1;
        var tot = _designs.Count();
        long sum = 0;


        foreach (var design in _designs)
        {

            var possible = PossibleToCreate(design.TargetPattern);
            var count = PossibleCombinations(design.TargetPattern);
            sum += count;
            design.Possible = possible;
            Console.WriteLine($"{i} - {possible} - {count}");

            i++;
        }

        Console.WriteLine(_designs.Count(d => d.Possible));
        Console.WriteLine(sum);
    }

    long PossibleCombinations(string target)
    {

        if (target == "")
        {
            return 1;
        }
        if (_possiblecombinationscache.ContainsKey(target))
            return _possiblecombinationscache[target];
        var matches = _towelPatterns.Where(p => target.StartsWith(p)).ToList();
        if (!matches.Any())
            return 0;
        var combinations = new List<bool>();
        long sum = 0;
        foreach (var match in matches)
        {
            var newTarget = target.Substring(match.Length);
            var res = PossibleCombinations(newTarget);
            if (!_possiblecombinationscache.ContainsKey(newTarget))
                _possiblecombinationscache.Add(newTarget, res);
            sum += res;
        }

        return sum;
    }

    bool PossibleToCreate(string target)
    {

        if (target == "")
        {
            return true;
        }
        if (_cancreatecache.ContainsKey(target))
            return _cancreatecache[target];
        var matches = _towelPatterns.Where(p => target.StartsWith(p)).ToList();
        if (!matches.Any())
            return false;

        foreach (var match in matches)
        {
            var newTarget = target.Substring(match.Length);
            var res = PossibleToCreate(newTarget);
            if (!_cancreatecache.ContainsKey(newTarget))
                _cancreatecache.Add(newTarget, res);
            if (res)

                return true;
        }

        return false;
    }
}

public class TowelDesign
{
    public required string TargetPattern { get; set; }

    public bool Possible;

}

