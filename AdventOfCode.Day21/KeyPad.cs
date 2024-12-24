using System.Drawing;

namespace AdventOfCode.Day21;

public class KeyPad
{
    public static readonly KeyPad NumericalKeypad = CreateNumericalKeypad();
    public static readonly KeyPad DirectionalKeypad = CreateDirectionalKeypad();

    private static KeyPad CreateDirectionalKeypad()
    {
        var keypad = new char?[3, 2];
        keypad[0, 0] = '<';
        keypad[1, 0] = 'v';
        keypad[2, 0] = '>';
        keypad[1, 1] = '^';
        keypad[2, 1] = 'A';
        return new KeyPad(keypad);
    }

    private static KeyPad CreateNumericalKeypad()
    {
        var keypad = new char?[3, 4];
        keypad[1, 0] = '0';
        keypad[2, 0] = 'A';
        keypad[0, 1] = '1';
        keypad[1, 1] = '2';
        keypad[2, 1] = '3';
        keypad[0, 2] = '4';
        keypad[1, 2] = '5';
        keypad[2, 2] = '6';
        keypad[0, 3] = '7';
        keypad[1, 3] = '8';
        keypad[2, 3] = '9';
        return new KeyPad(keypad);
    }


    private readonly char?[,] _keypad;

    public KeyPad(char?[,] keypad)
    {
        _keypad = keypad;
    }

    public Point Location(char character)
    {
        for (int x = 0; x < _keypad.GetLength(0); x++)
        {
            for (int y = 0; y < _keypad.GetLength(1); y++)
            {
                if (_keypad[x, y] == character)
                {
                    return new Point(x, y);
                }
            }
        }

        throw new Exception();
    }

    public bool OutOfBounds(Point point)
    {
        return _keypad.GetLength(0) <= point.X || _keypad.GetLength(1) <= point.Y ||
               point.X < 0 || point.Y < 0 || _keypad[point.X, point.Y] == null;
    }
}