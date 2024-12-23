using System.Drawing;

namespace AdventOfCode.Day20;

public class Part1
{
    public static void Run(string[] lines)
    {
        var (grid, scores, startPosition, endPosition) = Shared.InitialiseGrid(lines);

        // Now calculate cheats
        var cheats = new List<(Point StartLocation, Point EndLocation, int Score)>();

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] != '.' && grid[x,y] != 'S')
                {
                    continue;
                }

                var cheatStartPosition = new Point(x, y);

                foreach (var direction in Shared.Directions)
                {
                    var cheatSquarePosition = cheatStartPosition + direction;
                    if (grid[cheatSquarePosition.X, cheatSquarePosition.Y] != '#')
                    {
                        continue;
                    }

                    foreach (var secondDirection in Shared.Directions)
                    {
                        var cheatEndPosition = cheatSquarePosition + secondDirection;
                        if (Shared.IsOutOfBounds(grid, cheatEndPosition) || (grid[cheatEndPosition.X, cheatEndPosition.Y] != '.' && grid[cheatEndPosition.X, cheatEndPosition.Y] != 'E'))
                        {
                            continue;
                        }

                        var cheatStartScore = scores[cheatStartPosition.X, cheatStartPosition.Y];
                        var cheatEndScore = scores[cheatEndPosition.X, cheatEndPosition.Y];
                        var cheatScore = cheatEndScore - cheatStartScore - 2;
                        if (cheatScore > 0)
                        {
                            cheats.Add((cheatStartPosition, cheatEndPosition, cheatScore));
                        }
                    }
                }
            }
        }

        var cheatDistribution = cheats
            .GroupBy(c => c.Score)
            .Select(g => (CheatScore: g.Key, Count: g.Count()))
            .OrderBy(c => c.CheatScore)
            .ToArray();
        
        Console.WriteLine("Part 1");
        foreach (var cheatScoreCount in cheatDistribution)
        {
            Console.WriteLine($"There are {cheatScoreCount.Count} cheats that save {cheatScoreCount.CheatScore} picoseconds");
        }

        Console.WriteLine();
        var cheatsThatSave100SecondsOrMore = cheats.Count(c => c.Score >= 100);
        Console.WriteLine($"Scores that save at least 100 seconds: {cheatsThatSave100SecondsOrMore}");
        Console.WriteLine();
    }
}