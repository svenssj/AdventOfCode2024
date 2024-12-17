// // See https://aka.ms/new-console-template for more information

var d = new Day16();
d.Run();

// var program = "2,4,1,3,7,5,0,3,4,1,1,5,5,5,3,0";
// var output = "";


// // int i = 1;
// // int jumper = 1;
// // string prevmatch = "";
// // while (output != program)
// // {
// //     if (i == 45483412)
// //     {
// //         i++;
// //         continue;
// //     }

// //     var day = new Day17(new InitialState() { A = i, InitString = program });
// //     output = day.Run();


// //     if (output.EndsWith(prevmatch))
// //     {
// //         Thread.Sleep(50);
// //         Console.WriteLine(i + " - " + output);
// //     }

// //     if (program.EndsWith(output))
// //     {
// //         prevmatch = output;
// //         if (output == program)
// //             break;
// //         Console.WriteLine(i + " - " + output);
// //         jumper = i * 8;

// //         Console.WriteLine("Jumping to: " + jumper);
// //         Console.WriteLine($"That's {jumper - i} steps");

// //         i = jumper;

// //     }
// //     else if (output.EndsWith(prevmatch))
// //     {
// //         i++;
// //     }
// //     else
// //     {
// //         i += 8;
// //     }
// // }
// // Console.WriteLine("completed");


// var count = 0;
// var prev = 0;

// for (int j = 1610327; j < 11840327; j++)
// {
//     var day = new Day17(new InitialState() { A = j, InitString = program });
//     output = day.Run();
//     if (output.EndsWith("1,5,5,5,3,0"))
//     {
//         if(count==0)
//         {
//             Console.WriteLine($"Jumped {j - prev} steps");
//         }
//         count++;


//         Console.WriteLine(j + " - " + output);
//     }
//     else
//     {
//         if (count > 0)
//         {   prev = j;
//             Console.WriteLine("Segment: " + count);
//         }
//         count = 0;
//     }
// }




