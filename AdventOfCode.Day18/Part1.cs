using System.Drawing;

namespace AdventOfCode.Day18;

public class Part1
{
    public static void Run(Point[] fallingBytes, int bytes, Point goal)
    {
        var startPosition = new Point(0, 0);
        var grid = Shared.InitialiseGridWithBytes(fallingBytes, bytes, goal);

        Shared.OutputGrid(grid);

        var gridDistances = Shared.GetGridDistances(grid, startPosition);

        var exitDistance = gridDistances[goal.X,goal.Y];
        Console.WriteLine("Shortest distance: " + exitDistance);
    }
}