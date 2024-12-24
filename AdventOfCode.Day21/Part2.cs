namespace AdventOfCode.Day21;

public class Part2
{
    public static void Run(string[] codes)
    {
        var cache = new RobotCache();
        // Part 2
        List<(string Code, long ComplexityScore)> codeComplexityScore = [];
        foreach (var code in codes)
        {
            var shortestSequenceLength = FindShortestSequenceLength(code, cache);
            var codeNumberPart = long.Parse(code[..^1]);
            codeComplexityScore.Add((code, shortestSequenceLength * codeNumberPart));
        }

        var sum = codeComplexityScore.Sum(c => c.ComplexityScore);
        
        Console.WriteLine($"Sum: {sum}");
    }

    public static long FindShortestSequenceLength(string code, RobotCache cache)
    {
        var moves = GetNumericalKeypadSequences(code);
        var smallestSolution = long.MaxValue;
        foreach (var moveSequence in moves)
        {
            IEnumerable<(char LeftButton, char RightButton)> pairs = moveSequence.Append('A').Select((c, i) => (
                LeftButton: i == 0 ? 'A' : moveSequence[i - 1], 
                RightButton: c));
            var sum = pairs.Select(p => cache.CalculateLength(p.LeftButton, p.RightButton, 2)).Sum();
            if (sum < smallestSolution)
            {
                smallestSolution = sum;
            }
        }

        return smallestSolution;
    }
    
    public static string[] GetNumericalKeypadSequences(string inputs)
    {
        var startButton = 'A';
        string[] moves = [""];
        for (int i = 0; i < inputs.Length; i++)
        {
            var targetButton = inputs[i];

            var newMoves = RobotCache.GetMoves(startButton, targetButton, KeyPad.NumericalKeypad);
            startButton = targetButton;

            moves = (from move in moves
                from newMove in newMoves
                select (move + newMove + "A")).ToArray();
        }

        return moves;
    }
}