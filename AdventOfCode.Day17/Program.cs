using System.Drawing;
using System.Numerics;
using AdventOfCode.Day17;

// var inputFile = "example.txt";
// var inputFile = "part-2-example.txt";
var inputFile = "input.txt";

var lines = File.ReadLines(inputFile).ToArray();

var registerA = GetRegisterValue(lines[0]);
var registerB = GetRegisterValue(lines[1]);
var registerC = GetRegisterValue(lines[2]);

var program = GetProgram(lines[4]);
// Part 1
var computer = new Computer();
computer.SetInitialState(registerA, registerB, registerC, program);

while (computer.Tick())
{
    
}

Console.WriteLine(string.Join(",", computer.Output));

// Part2
Part2.Run(program, registerA, registerB, registerC);


static long[] GetProgram(string line)
{
    var index = line.IndexOf(":", StringComparison.Ordinal) + 1;
    var programInputs = line.Substring(index, line.Length - index);
    return programInputs.Split(",").Select(long.Parse).ToArray();
}

static long GetRegisterValue(string line)
{
    var index = line.IndexOf(":", StringComparison.Ordinal) + 1;
    var value = line.Substring(index, line.Length - index);
    return long.Parse(value);
}