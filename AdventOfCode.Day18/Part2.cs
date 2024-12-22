using System.Drawing;

namespace AdventOfCode.Day18;

public class Part2
{
    public static void Run(Point[] fallingBytes, Point goal)
    {
        // Simple idea - do a binary search to get the number of bytes, and then calculate the grid distances.
        // More efficient algorithms are of course available, but this will be good enough..!
        
        var startPosition = new Point(0, 0);

        var leftBytesIndex = 0;
        var rightBytesIndex = fallingBytes.Length - 1;
        var grid = new char [0,0];
        while (leftBytesIndex != rightBytesIndex)
        {
            var middleBytes = ((leftBytesIndex + rightBytesIndex) / 2) + 1;
            grid = Shared.InitialiseGridWithBytes(fallingBytes, middleBytes, goal);

            var gridDistances = Shared.GetGridDistances(grid, startPosition);

            var exitDistance = gridDistances[goal.X,goal.Y];
            
            // i.e. we couldn't reach it
            if (exitDistance == int.MaxValue)
            {
                rightBytesIndex = middleBytes - 1;
            }
            else
            {
                leftBytesIndex = middleBytes;
            }
        }
        
        Console.WriteLine($"Number of bytes until path closes: {leftBytesIndex + 1}");
        
        Console.WriteLine($"Byte that will cause path to be blocked: {fallingBytes[leftBytesIndex]}");
        
        //Shared.OutputGrid(grid);
    }
}