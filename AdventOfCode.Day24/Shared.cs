using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day24;

public class Shared
{
    public static (Dictionary<string, bool> StartingValues, List<Instruction> Instructions) ParseInput(string[] lines)
    {
        var values = new Dictionary<string, bool>();

        int i;
        for (i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            var parts = line.Split(": ");
            var key = parts[0];
            var value = parts[1] == "1" ? true : false;
            values[key] = value;
        }

        List<Instruction> instructions = [];
        for (int j = i + 1; j < lines.Length; j++)
        {
            var line = lines[j];
            var parts = line.Split(" ");
            var operand1 = parts[0];
            var logicGate = parts[1];
            var operand2 = parts[2];
            var operandResult = parts[4];

            var fullInstruction = new Instruction(operand1, operand2, operandResult, logicGate);
            instructions.Add(fullInstruction);
        }

        return (values, instructions);
    }
    
    private static readonly Regex ZOperandRegex = new ("^z[0-9]+$", RegexOptions.Compiled);

    public static string GetZData(Dictionary<string, bool> values)
    {
        var answer = new StringBuilder();
        foreach (var (key, value) in values.Where(v => ZOperandRegex.IsMatch(v.Key)).OrderByDescending(v => v.Key))
        {
            answer.Append(value ? '1' : '0');
        }

        return answer.ToString();
    }

    public static void OutputValues(Dictionary<string, bool> values)
    {
        foreach (var (key, value) in values.OrderBy(v => v.Key))
        {
            var valueString = value ? "1" : "0";
            Console.WriteLine($"{key}: {valueString}");
        }
    }

    public static bool RunBinaryLogic(bool leftValue, bool rightValue, string logicGate) =>
        logicGate switch
        {
            "AND" => leftValue && rightValue,
            "OR" => leftValue || rightValue,
            "XOR" => leftValue ^ rightValue,
            _ => throw new Exception("Unknown logic gate: " + logicGate)
        };

    public static void RunAllCalculations(Dictionary<string, bool> values, List<Instruction> instructions)
    {
        var calculated = true;
        while (calculated)
        {
            calculated = false;

            for (int i = instructions.Count - 1; i >= 0; i--)
            {
                var instruction = instructions[i];
                if (values.TryGetValue(instruction.Operand1, out var leftValue) &&
                    values.TryGetValue(instruction.Operand2, out var rightValue))
                {
                    var result = Shared.RunBinaryLogic(leftValue, rightValue, instruction.LogicGate);
                    values[instruction.ResultOperand] = result;
                    calculated = true;
                    instructions.RemoveAt(i);
                }
            }
        }
    }
}