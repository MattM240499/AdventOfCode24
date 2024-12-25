using System.Security.Cryptography;

namespace AdventOfCode.Day24;

public class Part1
{
    public static void Run(string[] lines)
    {
        var (values, instructions) = Shared.ParseInput(lines);

        Shared.RunAllCalculations(values, instructions);

        Shared.OutputValues(values);

        var zData = Shared.GetZData(values);
        Console.WriteLine($"Z data: {zData}");

        var decimalNumber = Convert.ToInt64(zData, 2);
        
        Console.WriteLine($"decimal number: {decimalNumber}");
    }
}

public record Instruction(string Operand1, string Operand2, string ResultOperand, string LogicGate);