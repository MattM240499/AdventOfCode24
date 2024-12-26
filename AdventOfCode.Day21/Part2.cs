namespace AdventOfCode.Day21;

public class Part2
{
    public static void Run(string[] codes)
    {
        var cache = new RobotSolver();
        // Part 2
        List<(string Code, long Length, long CodeNumber, long ComplexityScore)> codeComplexityScores = [];
        foreach (var code in codes)
        {
            var shortestSequenceLength = FindShortestSequenceLength(code, cache, 25);
            var codeNumberPart = long.Parse(code[..^1]);
            codeComplexityScores.Add((code, shortestSequenceLength, codeNumberPart, shortestSequenceLength * codeNumberPart));
        }

        foreach (var codeComplexityScore in codeComplexityScores)
        {
            Console.WriteLine($"Code: {codeComplexityScore.Code}: {codeComplexityScore.Length} * {codeComplexityScore.CodeNumber} = {codeComplexityScore.ComplexityScore}");
        }
        
        var sum = codeComplexityScores.Sum(c => c.ComplexityScore);
        
        Console.WriteLine($"Final sum: {sum}");
    }

    public static long FindShortestSequenceLength(string code, RobotSolver solver, int robots)
    {
        var numpadMoveSequences = GetNumericalKeypadSequences(code);
        var shortestSolution = long.MaxValue;
        foreach (var moveSequence in numpadMoveSequences)
        {
            var length = solver.CalculateOptimalLength(moveSequence, robots);
            if (length < shortestSolution)
            {
                shortestSolution = length;
            }
        }

        return shortestSolution;
    }
    
    public static string[] GetNumericalKeypadSequences(string inputs)
    {
        var startButton = 'A';
        string[] moves = [""];
        for (int i = 0; i < inputs.Length; i++)
        {
            var targetButton = inputs[i];

            var newMoves = RobotSolver.GetMoves(startButton, targetButton, KeyPad.NumericalKeypad);
            startButton = targetButton;

            moves = (from move in moves
                from newMove in newMoves
                select (move + newMove + "A")).ToArray();
        }

        return moves;
    }
}