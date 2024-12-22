using System.Collections.Frozen;
using System.Drawing;

namespace AdventOfCode.Day18;

public class Shared
{
    public static void OutputGrid(char[,] grid)
    {
        for (var y = 0; y < grid.GetLength(1); y++)
        {
            for (var x = 0; x < grid.GetLength(0); x++)
            {
                Console.Write(grid[x,y]);
            }
            Console.WriteLine();
        }
    }

    public static int[,] GetGridDistances(char[,] grid, Point startPosition)
    {
        {
            var ints = new int[grid.GetLength(0),grid.GetLength(1)];
            for (var y = 0; y < ints.GetLength(1); y++)
            {
                for (var x = 0; x < ints.GetLength(0); x++)
                {
                    ints[x, y] = int.MaxValue;
                }
            }

            ints[startPosition.X,startPosition.Y] = 0;
            var pointQueue = new Queue<Point>();
            pointQueue.Enqueue(startPosition);

            Size[] directions = [new(1, 0), new(-1, 0), new(0, -1), new(0, 1)];

            while (pointQueue.TryDequeue(out var point))
            {
                var pointDistance = ints[point.X, point.Y];
                foreach (var direction in directions)
                {
                    var newPoint = point + direction;
                    // If the point is not in bounds or a shorter route already found, do nothing with this connection
                    if (IsOutOfBounds(grid, newPoint) || ints[newPoint.X, newPoint.Y] <= pointDistance + 1)
                    {
                        continue;
                    }

                    ints[newPoint.X, newPoint.Y] = pointDistance + 1;
                    pointQueue.Enqueue(newPoint);
                }
            }

            return ints;
        }
    }

    public static bool IsOutOfBounds(char[,] grid,  Point point)
    {
        return grid.GetLength(0) <= point.X || grid.GetLength(1) <= point.Y ||
               point.X < 0 || point.Y < 0 || grid[point.X, point.Y] == '#';
    }

    public static char[,] InitialiseGridWithBytes(Point[] fallingBytes, int bytes, Point goal)
    {
        var bytesDropped = fallingBytes.Take(bytes).ToFrozenSet();

        var grid = new char[goal.X + 1, goal.Y + 1];
        for (var y = 0; y < grid.GetLength(1); y++)
        {
            for (var x = 0; x < grid.GetLength(0); x++)
            {
                if (bytesDropped.Contains(new Point(x, y)))
                {
                    grid[x, y] = '#';
                }
                else
                {
                    grid[x, y] = '.';
                }
            }
        }

        return grid;
    }
}