using System.Text;
using System.Text.RegularExpressions;


public class MulInstruction
{

    public MulInstruction(string input)
    {
        input = input.Replace("mul(", "");
        input = input.Replace(")", "");

        var splits = input.Split(',');

        A = int.Parse(splits[0]);
        B = int.Parse(splits[1]);

    }
    public int A { get; set; }
    public int B { get; set; }

    public int Result => A * B;
}

public class Day3
{

    public Day3()
    {

    }
    public void Run()
    {

        var list1 = new List<int>();
        var list2 = new List<int>();

        var input = File.ReadAllText("Day3\\input.txt");
        var instructions = new List<MulInstruction>();

        var doswithdonts = input.Split("do()");
        var dos = new List<string>();

        foreach (var dowithdont in doswithdonts)
        {
            var dont = "don't()";
            var idx = dowithdont.IndexOf(dont);
            if (idx == -1)
            {
                dos.Add(dowithdont);
            }
            else
                dos.Add(dowithdont.Remove(idx));
        }



        var regex = new Regex("mul\\(\\d{1,3},\\d{1,3}\\)");


        foreach (var doString in dos)
        {
            var matches = regex.Matches(doString);

            foreach (var match in matches)
            {
                if(match==null)
                    throw new NullReferenceException();
                Console.WriteLine(match.ToString());
                instructions.Add(new MulInstruction(match.ToString()));
            }
        }


        var sum = instructions.Sum(m => m.Result);

    }
}