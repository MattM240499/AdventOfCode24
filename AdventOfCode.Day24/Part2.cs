using System.Text;

namespace AdventOfCode.Day24;

public class Part2
{
    public static void Run(string[] lines)
    {
        // Looks like a brute force approach will be too slow ~ 10^16 iterations required.
        // Idea is to look at the sequence of moves for each z value and then identify any suspicious looking ones
        
        //Part2FullAdderVerificationAttempt(zValues, xValues, yValues, instructions);

        var (inputValues, inputInstructions) = Shared.ParseInput(lines);
        
        var expectedAnswer = GetExpectedAnswer(inputValues);
        
        Part2FullAdderVerificationAttempt(inputValues, inputInstructions);
    }

    private static long GetExpectedAnswer(Dictionary<string, bool> values)
    {
        var xData = GetData(values, 'x');
        var yData = GetData(values, 'y');

        var xInput = Convert.ToInt64(xData, 2);
        var yInput = Convert.ToInt64(yData, 2);
        var expectedAnswer = xInput + yInput;
        return expectedAnswer;
    }

    private static void Part2FullAdderVerificationAttempt(Dictionary<string, bool> values,
        List<Instruction> instructions)
    {
        var xValues = values.Keys.Where(v => v.StartsWith('x')).OrderBy(x => x).ToArray();
        var yValues = values.Keys.Where(v => v.StartsWith('y')).OrderBy(y => y).ToArray();
        var zValues = instructions.Where(i => i.ResultOperand.StartsWith('z')).Select(i => i.ResultOperand)
            .OrderBy(z => z)
            .ToArray();
        // Perhaps we just verify the structure of the full adder...?
        
        // Manually check the first one
        string? cIn = null;
        List<(string, string)> swaps = [];
        for (int i = 0; i < xValues.Length; i++)
        {
            var aIn = xValues[i];
            var bIn = yValues[i];

            AdderCheckResult result;
            if(i == 0)
            {
                result = CheckHalfAdder(aIn, bIn, instructions, zValues[i]);
            }
            else
            {
                result = CheckFullAdder(aIn, bIn, cIn!, instructions, zValues[i]);
            }
            
            if (result.Valid)
            {
                cIn = result.CarryOperand;
            }
            else
            {
                var swap = result.Swap!.Value;
                // Do the swap, then go again
                SwapInstructions(instructions, swap.instruction1, swap.instruction2);
                // In case we swap the carry, change it here.
                if (swap.instruction1.ResultOperand == cIn)
                {
                    cIn = swap.instruction2.ResultOperand;
                }
                else if (swap.instruction2.ResultOperand == cIn)
                {
                    cIn = swap.instruction1.ResultOperand;
                }
                i--;
                swaps.Add((swap.instruction1.ResultOperand, swap.instruction2.ResultOperand));
            }
        }
        
        Console.WriteLine("Output: " + string.Join(",", swaps.SelectMany(s => new[]{s.Item1, s.Item2}).OrderBy(s => s)));
    }

    private static void SwapInstructions(List<Instruction> instructions, Instruction instruction1, Instruction instruction2)
    {
        var index1 = instructions.IndexOf(instruction1);
        var index2 = instructions.IndexOf(instruction2);
        instructions[index1] = instruction1 with { ResultOperand = instruction2.ResultOperand };
        instructions[index2] = instruction2 with { ResultOperand = instruction1.ResultOperand };
    }

    private static AdderCheckResult CheckFullAdder(string aIn, string bIn, string cIn, List<Instruction> instructions, string expectedSumOperand)
    {
        // ReSharper disable InconsistentNaming
        var aXORb = FindXor(instructions, aIn, bIn) ?? throw new Exception("Should be impossible");
        var aANDb = FindAnd(instructions, aIn, bIn) ?? throw new Exception("Should be impossible");

        var aXORbXORc = FindXor(instructions, aXORb.ResultOperand, cIn);
        var aXORbANDc = FindAnd(instructions, aXORb.ResultOperand, cIn);
        
        // If aXorBXorC and aXorBAndC instructions both exist, they can stay as is
        if (aXORbXORc == null || aXORbANDc == null)
        {
            // If only one exists this would be very strange as we always do an AND and XOR with the same inputs
            if (aXORbXORc == null ^ aXORbANDc == null)
            {
                throw new Exception();
            }
            
            // Try and find one that is already in place
            var xor = FindXor(instructions, aXORb.ResultOperand);
            var cInOutputResult = FindOutputInstruction(instructions, cIn);
            if (xor != null)
            {
                // Swap the AXORb output with 
                var findOutputResult = FindOtherOperandOutputInstruction(instructions, xor, aXORb.ResultOperand);
                return AdderCheckResult.RequiresSwap(cInOutputResult, findOutputResult);
            }

            xor = FindXor(instructions, cIn);
            if (xor != null)
            {
                // Swap carry output with another matching operand
                var findOutputResult = FindOtherOperandOutputInstruction(instructions, xor, cIn);
                return AdderCheckResult.RequiresSwap(aXORb, findOutputResult);
            }

            if (xor == null) throw new Exception("Rare case where both the carry and the aXORb both need to be swapped. Praying we don't hit this...");
        }

        if (aXORbXORc!.ResultOperand != expectedSumOperand)
        {
            var outputInstruction = FindOutputInstruction(instructions, expectedSumOperand);
            return AdderCheckResult.RequiresSwap(aXORbXORc, outputInstruction);
        }
        
        // Final bit, the carry
        var finalOr = FindOr(instructions, aANDb.ResultOperand, aXORbANDc.ResultOperand);
        if (finalOr == null)
        {
            finalOr = FindOr(instructions, aANDb.ResultOperand);
            if (finalOr != null)
            {
                var outputInstruction = FindOtherOperandOutputInstruction(instructions, finalOr, aANDb.ResultOperand);
                return AdderCheckResult.RequiresSwap(aANDb, outputInstruction);
            }

            finalOr = FindOr(instructions, aXORbANDc.ResultOperand);
            if (finalOr != null)
            {
                var outputInstruction = FindOtherOperandOutputInstruction(instructions, finalOr, aXORbANDc.ResultOperand);
                return AdderCheckResult.RequiresSwap(aXORbANDc, outputInstruction);
            }

            if (finalOr == null)
                throw new Exception(
                    "Rare case where both and outputs need to be swapped. Praying we don't hit this...");
        }

        return AdderCheckResult.Ok(finalOr.ResultOperand);
    }

    private static Instruction FindOtherOperandOutputInstruction(List<Instruction> instructions,
        Instruction instruction, string operand)
    {
        var otherOperand = instruction.Operand1;
        if (otherOperand == operand) otherOperand = instruction.Operand2;
        var findOutputResult = FindOutputInstruction(instructions, otherOperand);
        return findOutputResult;
    }

    private static AdderCheckResult CheckHalfAdder(string aIn, string bIn, List<Instruction> instructions, string expectedOutput)
    {
        var aBXor = FindXor(instructions, aIn, bIn) ?? throw new Exception("Should be impossible");
        var aBAnd = FindAnd(instructions, aIn, bIn) ?? throw new Exception("Should be impossible");
        var sum = aBXor.ResultOperand;
        var carry = aBAnd.ResultOperand;
        if (sum != expectedOutput)
        {
            var output = FindOutputInstruction(instructions, expectedOutput);
            return AdderCheckResult.RequiresSwap(aBAnd, output);
        }

        return AdderCheckResult.Ok(carry);
    }

    private static Instruction FindOutputInstruction(List<Instruction> instructions, string expectedOutput)
    {
        return instructions.First(i => i.ResultOperand == expectedOutput);
    }

    private static Instruction? FindXor(List<Instruction> instructions, string aIn, string bIn)
    {
        return FindGate(instructions, aIn, bIn, "XOR");
    }
    
    private static Instruction? FindAnd(List<Instruction> instructions, string aIn, string bIn)
    {
        return FindGate(instructions, aIn, bIn, "AND");
    }
    
    private static Instruction? FindOr(List<Instruction> instructions, string aIn, string bIn)
    {
        return FindGate(instructions, aIn, bIn, "OR");
    }
    
    private static Instruction? FindXor(List<Instruction> instructions, string aIn)
    {
        return FindGate(instructions, aIn, "XOR");
    }
    
    private static Instruction? FindOr(List<Instruction> instructions, string aIn)
    {
        return FindGate(instructions, aIn, "OR");
    }

    private static Instruction? FindGate(List<Instruction> instructions, string aIn, string bIn, string gate)
    {
        return instructions.FirstOrDefault(i => ((i.Operand1 == aIn && i.Operand2 == bIn) || (i.Operand1 == bIn && i.Operand2 == aIn)) && i.LogicGate == gate);
    }
    
    private static Instruction? FindGate(List<Instruction> instructions, string aIn, string gate)
    {
        return instructions.FirstOrDefault(i => (i.Operand1 == aIn || i.Operand2 == aIn) && i.LogicGate == gate);
    }

    public static string GetData(Dictionary<string, bool> values, char identifier)
    {
        var answer = new StringBuilder();
        foreach (var (key, value) in values.Where(v => v.Key.StartsWith(identifier)).OrderByDescending(v => v.Key))
        {
            answer.Append(value ? '1' : '0');
        }

        return answer.ToString();
    }
}

public record AdderCheckResult(
    bool Valid,
    string? CarryOperand,
    (Instruction instruction1, Instruction instruction2)? Swap)
{
    public static AdderCheckResult RequiresSwap(Instruction instruction1, Instruction instruction2)
    {
        return new AdderCheckResult(false, null, (instruction1, instruction2));
    }

    public static AdderCheckResult Ok(string carryOperand)
    {
        return new AdderCheckResult(true, carryOperand, null);
    }
}