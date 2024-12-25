using Microsoft.VisualBasic;

public class Day22
{

    List<Secret> secrets;
    public Day22()
    {
        var lines = File.ReadAllLines("Day22\\input.txt");
        secrets = [];
        foreach (var line in lines)
        {
            secrets.Add(new Secret() { Init = long.Parse(line) });
        }
    }

    public void Run()
    {

        var completed = 0;
        var total = secrets.Count;
        HashSet<string> allKeys = [];
        foreach (var secret in secrets)
        {

            Console.WriteLine($"{completed}/{total}");
            completed++;
            var curr = secret.Init;

            var last = int.Parse(curr.ToString().Last().ToString());
            var prev = last;

            secret.OnesByDiffs.Add(string.Join(',', secret.Diffs.Skip(Math.Max(0, secret.Diffs.Count))), last);
            for (int i = 0; i < 2000; i++)
            {

                var tmp = curr * 64;
                curr = Mix(curr, tmp);
                curr = Prune(curr);

                tmp = curr / 32;
                curr = Mix(curr, tmp);
                curr = Prune(curr);


                tmp = curr * 2048;
                curr = Mix(curr, tmp);
                curr = Prune(curr);

                last = int.Parse(curr.ToString().Last().ToString());
                secret.Diffs.Add(last - prev);
                var key = string.Join(',', secret.Diffs.Skip(Math.Max(0, secret.Diffs.Count - 4)));
                if (!secret.OnesByDiffs.ContainsKey(key))
                    secret.OnesByDiffs.Add(key, last);


                prev = last;
            }

            allKeys.UnionWith(secret.GetDiffKeys());
            secret.Value = curr;

        }

        var best = 0;

        foreach (var key in allKeys.Skip(4))
        {
            var sum = secrets.Sum(s => s.GetValueByKey(key));
            if (sum > best)
            {
                Console.WriteLine(key +":  "+ sum);
                best = sum;
            }
        }


    }


    long Mix(long a, long b)
    {

        var aBits = ToBits(a);
        var bBits = ToBits(b);

        var res = new List<bool>();

        var len = bBits.Length;


        for (int i = 0; i < len; i++)
        {
            if (bBits[i] != aBits[i])
                res.Add(true);
            else
                res.Add(false);
        }

        return ToLong(res.ToArray());

    }

    long Prune(long val)
    {
        return val % 16777216;
    }


    public bool[] ToBits(long l)
    {

        var longString = Convert.ToString(l, 2);

        var diff = 64 - longString.Length;
        for (int i = 0; i < diff; i++)
        {
            longString = '0' + longString;
        }

        var bools = new bool[64];

        for (int i = 0; i < 64; i++)
        {
            bools[i] = longString[i] == '1';
        }

        return bools;

    }

    public long ToLong(bool[] bits)
    {

        var s = "";
        foreach (var bit in bits)
        {
            s += bit ? "1" : "0";
        }

        var res = Convert.ToInt64(s, 2);
        return res;
    }
}

public class Secret
{
    public Secret()
    {
        Diffs = [];
        OnesByDiffs = [];
    }
    public long Init { get; set; }
    public long Value { get; set; }

    public List<int> Diffs { get; set; }
    public Dictionary<string, int> OnesByDiffs { get; set; }

    public HashSet<string> GetDiffKeys()
    {
        return OnesByDiffs.Keys.ToHashSet();
    }

    public int GetValueByKey(string key)
    {
        if (!OnesByDiffs.ContainsKey(key))
            return 0;
        return OnesByDiffs[key];
    }
}


