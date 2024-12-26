using System.Diagnostics;
using AdventOfCode.Day25;

// var inputFile = "example.txt";
var inputFile = "input.txt";

var lines = File.ReadLines(inputFile).ToArray();

RunPart1();
// RunPart2();

void RunPart1()
{
    var sw = Stopwatch.StartNew();
    Part1.Run(lines);

    sw.Stop();
    Console.WriteLine($"Part 1 in {sw.Elapsed}");
}

void RunPart2()
{
    var sw = Stopwatch.StartNew();
    Part2.Run(lines);

    sw.Stop();
    Console.WriteLine($"Part 2 in {sw.Elapsed}");
}

