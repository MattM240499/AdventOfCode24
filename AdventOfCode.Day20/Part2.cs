using System.Drawing;

namespace AdventOfCode.Day20;

public class Part2
{
    public static void Run(string[] lines)
    {
        var (grid, scores, startPosition, endPosition) = Shared.InitialiseGrid(lines);

        var maxScore = scores[endPosition.X, endPosition.Y];

        var raceTrackSquares = new Point[maxScore + 1];
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var score = scores[x, y];
                if (score != int.MaxValue)
                {
                    raceTrackSquares[score] = new Point(x, y);
                }
            }
        }
        // Idea - Work out between every two squares on the track whether a cheat exists.
        // To save time, we know that a cheat must have the following characteristics
        // - The score difference between the two squares must be greater than the distance between them
        // - The distance between two squares must be at most 20 squares apart
        // - The score in the end square must be higher than the start square

        // Now calculate cheats
        var cheats = new List<(Point StartLocation, Point EndLocation, int Score)>();

        for (int startSquareIndex = 0; startSquareIndex < raceTrackSquares.Length - 1; startSquareIndex++)
        {
            // + 3 because if it's +2 there's no way to cheat and gain time
            for (int endSquareIndex = startSquareIndex + 3; endSquareIndex < raceTrackSquares.Length; endSquareIndex++)
            {
                var startSquare = raceTrackSquares[startSquareIndex];
                var endSquare = raceTrackSquares[endSquareIndex];
                
                var cheatLength = Math.Abs(startSquare.X - endSquare.X) + Math.Abs(startSquare.Y - endSquare.Y);
                if (cheatLength == endSquareIndex - startSquareIndex)
                {
                    // Already optimal, no way to cheat
                    continue;
                }

                
                if (cheatLength > 20)
                {
                    // Cheat too long
                    continue;
                }

                var cheatScore = endSquareIndex - startSquareIndex - cheatLength;
                cheats.Add((startSquare, endSquare, cheatScore));
            }
        }

        var cheatDistribution = cheats
            .GroupBy(c => c.Score)
            .Select(g => (CheatScore: g.Key, Count: g.Count()))
            .OrderBy(c => c.CheatScore)
            .ToArray();

        Console.WriteLine();
        Console.WriteLine("Part 2");
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