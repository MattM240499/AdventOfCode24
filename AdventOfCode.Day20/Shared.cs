using System.Drawing;

namespace AdventOfCode.Day20;

public class Shared
{
    public static readonly IReadOnlyCollection<Size> Directions = [new(1, 0), new(0, 1), new(-1, 0), new(0, -1)];
    public static (char[,] Grid, int[,] Scores, Point StartPosition, Point EndPosition) InitialiseGrid(string[] lines)
    {
        var grid = new char[lines[0].Length, lines.Length];
        var scores = new int[lines[0].Length, lines.Length];
        Point startPosition = new (), endPosition = new ();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                grid[x, y] = lines[y][x];
                scores[x, y] = Int32.MaxValue;

                if (grid[x, y] == 'S')
                {
                    startPosition = new Point(x, y);
                }
                else if (grid[x, y] == 'E')
                {
                    endPosition = new Point(x, y);
                }
            }
        }

        var currentPosition = startPosition;
        var score = 0;
        Size? previousDirection = null;
        while (currentPosition != endPosition)
        {
            scores[currentPosition.X, currentPosition.Y] = score;
            
            foreach (var direction in Directions)
            {
                // Don't go back to the same square from which we came from
                if (previousDirection == new Size(direction.Width * -1, direction.Height * -1))
                {
                    continue;
                }
                var nextPosition = currentPosition + direction;
                if (grid[nextPosition.X, nextPosition.Y] == '#')
                {
                    continue;
                }
        
                if (grid[nextPosition.X, nextPosition.Y] == '.' || grid[nextPosition.X, nextPosition.Y] == 'E')
                {
                    previousDirection = direction;
                    score++;
                    currentPosition = nextPosition;
                    break;
                }
            }
        }
        
        scores[currentPosition.X, currentPosition.Y] = score;
        return (grid, scores, startPosition, endPosition);
    }
    
    public static bool IsOutOfBounds(char[,] grid, Point point)
    {
        return grid.GetLength(0) <= point.X || grid.GetLength(1) <= point.Y ||
               point.X < 0 || point.Y < 0;
    }
}