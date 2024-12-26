using System.Drawing;

namespace AdventOfCode.Day21;

public class RobotSolver
{
    public Dictionary<(char, char), string[]> PairPaths { get; set; }
    public Dictionary<(char, char, int), long> OptimalLengths { get; set; }

    private static readonly char[] Buttons = ['^', 'v', '>', '<', 'A'];

    private static readonly (char LeftButton, char RightButton)[] ButtonPairs =
        (from button1 in Buttons
            from button2 in Buttons
            select (button1, button2)).ToArray();

    public RobotSolver()
    {
        OptimalLengths = new();
        PairPaths = new();

        foreach (var (leftButton, rightButton) in ButtonPairs)
        {
            var optimalKeypadMoves = GetMoves(leftButton, rightButton, KeyPad.DirectionalKeypad);
            PairPaths[(leftButton, rightButton)] = optimalKeypadMoves.ToArray();
        }
    }

    public long CalculateOptimalLength(string sequence, int robots)
    {
        // The optimal length of a given sequence and robots is equal to the sum of optimal sequences
        // That move the robot between each button in the sequence
        long length = 0;
        for (int i = 0; i < sequence.Length; i++)
        {
            var start = i == 0 ? 'A' : sequence[i - 1];
            var end = sequence[i];
            length += CalculateOptimalLength(start, end, robots);
        }

        return length;
    }

    /// <summary>
    /// Calculate the optimal path length for robot <see cref="robots"/> to direct robot 0 to get from the <see cref="startButton"/> to the
    /// <see cref="endButton"/>
    /// </summary>
    private long CalculateOptimalLength(char startButton, char endButton, int robots)
    {
        if (robots == 0) throw new ArgumentException("Robots must be greater than zero");
        // Basic idea is to do a dfs search for all sequences with results caching
        if (OptimalLengths.TryGetValue((startButton, endButton, robots), out var optimalSolution))
        {
            return optimalSolution;
        }
        
        var moves = PairPaths[(startButton, endButton)];

        if (robots == 1)
        {
            // Robot 1 should be equal to the cost of moving robot 0 from the square to the new square then pressing A.
            return moves.Min(m => m.Length) + 1;
        }

        var shortestSolution = long.MaxValue;
        // Generate all moves for robot 0 to get from start button -> end button.
        foreach (var moveSequence in moves)
        {
            // In the example of robot 25...
            // Then the optimal move sequence for robot 25 that gets robot 0 from the start button to the end button,
            // will be the optimal move sequence that controls robot 24 to control robot 0 to move to the required square, then press 'A'
            long moveSequenceLength = CalculateOptimalLength(moveSequence + 'A', robots - 1);

            if (moveSequenceLength < shortestSolution)
            {
                shortestSolution = moveSequenceLength;
            }
        }
        OptimalLengths.Add((startButton, endButton, robots), shortestSolution);
        return shortestSolution;
    }

    public static IEnumerable<string> GetMoves(char leftButton, char rightButton, KeyPad keypad)
    {
        var startPoint = keypad.Location(leftButton);
        var endPoint = keypad.Location(rightButton);
        var stack = new Stack<(Point point, List<char> path)>();
        stack.Push((startPoint, []));
        while (stack.Count > 0)
        {
            var (point, path) = stack.Pop();
            if (point == endPoint)
            {
                yield return new string(path.ToArray());
                continue;
            }
            if (keypad.OutOfBounds(point))
                continue;
            if (point.X < endPoint.X)
                stack.Push((point with { X = point.X + 1 }, path.Append('>').ToList()));
            if (point.X > endPoint.X)
                stack.Push((point with {X = point.X - 1}, path.Append('<').ToList()));
            if (point.Y < endPoint.Y)
                stack.Push((point with { Y = point.Y + 1 }, path.Append('^').ToList()));
            if (point.Y > endPoint.Y)
                stack.Push((point with { Y = point.Y - 1 }, path.Append('v').ToList()));
        }
    }

}