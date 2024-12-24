using System.Drawing;

namespace AdventOfCode.Day21;

public class Shared
{
    public static readonly IReadOnlyCollection<Size> Directions = [new(1, 0), new(0, 1), new(-1, 0), new(0, -1)];
    
    public static Point GetNumericalKeypadPosition(char button)
    {
        return button switch
        {
            '0' => new Point(1, 0),
            'A' => new Point(2, 0),
            '1' => new Point(0, 1),
            '2' => new Point(1, 1),
            '3' => new Point(2, 1),
            '4' => new Point(0, 2),
            '5' => new Point(1, 2),
            '6' => new Point(2, 2),
            '7' => new Point(0, 3),
            '8' => new Point(1, 3),
            '9' => new Point(2, 3),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static bool IsOutOfNumericalKeypadBounds(Point point)
    {
        return point.X < 0 || point.Y < 0 || point.X > 3 || point.Y > 3 || point is { X: 0, Y: 0 };
    }
    
    public static Point GetDirectionKeypadPosition(char button)
    {
        return button switch
        {
            '<' => new Point(0, 0),
            'v' => new Point(1, 0),
            '>' => new Point(2, 0),
            '^' => new Point(1, 1),
            'A' => new Point(2, 1),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    public static char GetDirectionKeypadCharacter(Size direction)
    {
        if (direction == new Size(1, 0)) return '>';
        if (direction == new Size(-1, 0)) return '<';
        if (direction == new Size(0, 1)) return '^';
        if (direction == new Size(0, -1)) return 'v';
        throw new ArgumentOutOfRangeException();
    }

    public static bool IsOutOfDirectionKeypadBounds(Point point)
    {
        return point.X < 0 || point.Y < 0 || point.X > 2 || point.Y > 1 || point is { X: 0, Y: 1 };
    }
}