// var inputFile = "example.txt";
// var inputFile = "part-2-example.txt";

using System.Collections.Frozen;
using System.Drawing;
using AdventOfCode.Day18;

// Example
// var inputFile = "example.txt";
// var goal = new Point(6, 6);
// var bytes = 12;
var inputFile = "input.txt";
var goal = new Point(70, 70);
var bytes = 1024;

var lines = File.ReadLines(inputFile).ToArray();

var fallingBytes = lines
    .Select(l => l.Split(","))
    .Select(p => new Point(int.Parse(p[0]), int.Parse(p[1])))
    .ToArray();

//Part1.Run(fallingBytes, bytes, goal);
Part2.Run(fallingBytes, goal);