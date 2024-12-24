using System.Drawing;

namespace AdventOfCode.Day21;

public class RobotCache
{
    public Dictionary<(char, char), string[]> PairPaths { get; set; }
    public Dictionary<(char, char, int), long> OptimalSolutions { get; set; }
    
    private static readonly char[] Buttons = ['^', 'v', '>', '<', 'A'];

    private static readonly (char LeftButton, char RightButton)[] ButtonPairs =
        (from button1 in Buttons
            from button2 in Buttons
            select (button1, button2)).ToArray();

    public RobotCache()
    {
        OptimalSolutions = new();
        PairPaths = new();
        
        foreach (var (leftButton, rightButton) in ButtonPairs)
        {
            var optimalKeypadMoves = GetMoves(leftButton, rightButton, KeyPad.DirectionalKeypad);
            PairPaths[(leftButton, rightButton)] = optimalKeypadMoves.ToArray();
        }
    }

    public long CalculateLength(char leftButton, char rightButton, int depth)
    {
        if (OptimalSolutions.TryGetValue((leftButton, rightButton, depth), out var optimalSolution))
        {
            return optimalSolution;
        }

        var smallestSolution = long.MaxValue;
        var moves = PairPaths[(leftButton, rightButton)];
        if (depth == 0)
        {
            return 1;
        }

        foreach (var moveSequence in moves)
        {
            var pairs = moveSequence.Append('A').Select((c, i) => (a: i == 0 ? 'A' : moveSequence[i - 1], b: c)).ToArray();
            var sum = pairs.Select(p => CalculateLength(p.a, p.b, depth - 1)).Sum();
            if (sum < smallestSolution)
            {
                smallestSolution = sum;
            }
        }
        OptimalSolutions.Add((leftButton, rightButton, depth), smallestSolution);
        return smallestSolution;
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