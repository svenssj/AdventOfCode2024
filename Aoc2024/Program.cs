// // See https://aka.ms/new-console-template for more information

// var d = new Day16();
// d.Run();

var program = "2,4,1,3,7,5,0,3,4,1,1,5,5,5,3,0";
var output = "";



int jumper = 1;
string prevmatch = "";
List<int> thingsToTry = [1];

while (thingsToTry.Any())
{
    var current = thingsToTry.First();
    thingsToTry.RemoveAt(0);
    for (int i = 0; i < 8; i++)
    {
        var day = new Day17(new InitialState() { A = current + i, InitString = program });
        output = day.Run();
        if (program.EndsWith(output))
        {
            thingsToTry.Add((current + i) * 8);
            Console.WriteLine(i + " - " + output);
            if (output == program)
                break;
        }
       
          
    }

}
Console.WriteLine("completed");


// var count = 0;
// var prev = 0;

// for (int j = 0; j < 10; j++)
// {
//     var day = new Day17(new InitialState() { A = j, InitString = program });
//     output = day.Run();
//     Console.WriteLine(output);

// }




