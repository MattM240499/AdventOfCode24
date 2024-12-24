using System.Drawing;
using System.Text;

namespace AdventOfCode.Day21;

public class Part1
{
    public static void Run(string[] codes)
    {
        // Part 1
        List<(string Code, int ComplexityScore)> codeComplexityScore = [];
        foreach (var code in codes)
        {
            var shortestSequenceLength = FindShortestSequenceLength(code);
            var codeNumberPart = int.Parse(code[..^1]);
            codeComplexityScore.Add((code, shortestSequenceLength * codeNumberPart));
        }

        var sum = codeComplexityScore.Sum(c => c.ComplexityScore);
        
        Console.WriteLine($"Sum: {sum}");
    }

    private static int FindShortestSequenceLength(string code)
    {
        var robot1Sequences = GetOptimalNumericalKeypadSequences(code);

        List<string> robot2Sequences = [];
        foreach (var sequence in robot1Sequences)
        {
            var sequenceRobot2Sequences = GetOptimalDirectionKeypadSequence(sequence);
            robot2Sequences.AddRange(sequenceRobot2Sequences);
        }

        List<string> robot3Sequences = [];
        foreach (var sequence in robot2Sequences)
        {
            var sequenceRobot3Sequences = GetOptimalDirectionKeypadSequence(sequence);
            robot3Sequences.AddRange(sequenceRobot3Sequences);
        }

        return robot3Sequences.Min(r => r.Length);
    }

    public static string[] GetOptimalNumericalKeypadSequences(string inputs)
    {
        var startPosition = Shared.GetNumericalKeypadPosition('A');
        string[] moves = [""];
        for (int i = 0; i < inputs.Length; i++)
        {
            var targetButton = inputs[i];

            var targetPosition = Shared.GetNumericalKeypadPosition(targetButton);

            var newMoves = GetAllOptimalNumericalKeypadMoves(startPosition, targetPosition);
            startPosition = targetPosition;

            moves = (from move in moves
            from newMove in newMoves
            select (move + newMove + "A")).ToArray();
        }

        return moves;
    }

    public static string[] GetOptimalDirectionKeypadSequence(string inputs)
    {
        var startPosition = Shared.GetDirectionKeypadPosition('A');
        string[] moves = [""];
        for (int i = 0; i < inputs.Length; i++)
        {
            var targetButton = inputs[i];

            var targetPosition = Shared.GetDirectionKeypadPosition(targetButton);

            var newMoves  = GetOptimalDirectionKeypadMoves(startPosition, targetPosition);
            startPosition = targetPosition;

            moves = (from move in moves
                from newMove in newMoves
                select (move + newMove + "A")).ToArray();
        }

        return moves;
    }

    private static IEnumerable<string> GetAllOptimalNumericalKeypadMoves(Point startPoint, Point endPoint)
    {
        return GetOptimalKeypadMoves(startPoint, endPoint, Shared.IsOutOfNumericalKeypadBounds);
    }

    private static IEnumerable<string> GetOptimalDirectionKeypadMoves(Point startPoint, Point endPoint)
    {
        return GetOptimalKeypadMoves(startPoint, endPoint, Shared.IsOutOfDirectionKeypadBounds);
    }
    
    private static IEnumerable<string> GetOptimalKeypadMoves(Point startPoint, Point endPoint, Func<Point, bool> isOutOfBounds)
    {
        var xDistance = endPoint.X - startPoint.X;
        var yDistance = endPoint.Y - startPoint.Y;

        if (xDistance == 0 && yDistance == 0)
        {
            yield return "";
            yield break;
        }

        var xAbsoluteDistance = Math.Abs(xDistance);
        var yAbsoluteDistance = Math.Abs(yDistance);

        char? xMove = xAbsoluteDistance == 0 ? null : Shared.GetDirectionKeypadCharacter(new Size(xDistance / xAbsoluteDistance, 0));
        char? yMove = yAbsoluteDistance == 0 ? null : Shared.GetDirectionKeypadCharacter(new Size(0, yDistance / yAbsoluteDistance));

        // Basically, only consider move orders that use consecutive moves optimally.
        // Otherwise it is guaranteed to be more wasteful
        if (!isOutOfBounds(startPoint + new Size(xDistance, 0)) && xAbsoluteDistance != 0)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < xAbsoluteDistance; i++)
            {
                sb.Append(xMove!.Value);
            }
            for (int i = 0; i < yAbsoluteDistance; i++)
            {
                sb.Append(yMove!.Value);
            }

            yield return sb.ToString();
        }
        if(!isOutOfBounds(startPoint + new Size(0, yDistance)) && yAbsoluteDistance != 0)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < yAbsoluteDistance; i++)
            {
                sb.Append(yMove!.Value);
            }
            for (int i = 0; i < xAbsoluteDistance; i++)
            {
                sb.Append(xMove!.Value);
            }

            yield return sb.ToString();
        }
    }
}