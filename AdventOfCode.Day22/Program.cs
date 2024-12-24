using AdventOfCode.Day22;

// var inputFile = "example.txt";
// var inputFile = "part-1-example.txt";
// var inputFile = "part-2-example.txt";
var inputFile = "input.txt";

var lines = File.ReadLines(inputFile).ToArray();

var buyerStartSecretValues = lines.Select(ulong.Parse).ToArray();

// Part1.Run(buyerStartSecretValues);
Part2.Run(buyerStartSecretValues);