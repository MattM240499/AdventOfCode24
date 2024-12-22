using System.Drawing;

// var inputFile = "example.txt";
// var inputFile = "example2.txt";
var inputFile = "input.txt";

var lines = File.ReadLines(inputFile).ToArray();

var grid = new char[lines[0].Length,lines.Length];
for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines[0].Length; x++)
    {
        grid[x, y] = lines[y][x];
    }
}

var startingPosition = GetStartingPosition(grid);
var startingDirection = new Point(1, 0);
var endPosition = GetEndPosition(grid);

var positionsWithScores = new Dictionary<(Point Position, Point Direction), int>
{
    [(startingPosition, startingDirection)] = 0
};

List<(Point Position, Point Direction, int Score)> activePositions = [(startingPosition, startingDirection, 0)];

while (activePositions.Count > 0)
{
    List<(Point Position, Point Direction, int Score)> newActivePositions = [];
    foreach (var position in activePositions)
    {
        if (position.Position == endPosition)
        {
            continue;
        }

        List<(Point Position, Point Direction, int Score)> possibleMoves =
        [
            (position.Position + new Size(position.Direction), position.Direction, position.Score + 1),
            (position.Position, RotateClockwise(position.Direction), position.Score + 1000),
            (position.Position, RotateAntiClockwise(position.Direction), position.Score + 1000)
        ];

        foreach (var move in possibleMoves)
        {
            var positionValue = grid[move.Position.X, move.Position.Y];
            if (positionValue == '#')
            {
                continue;
            }
            
            if (positionsWithScores.TryGetValue((move.Position, move.Direction), out var bestScoreSoFar))
            {
                if (move.Score >= bestScoreSoFar)
                {
                    continue;
                }
            }
            
            positionsWithScores[(move.Position, move.Direction)] = move.Score;
            newActivePositions.Add((move.Position, move.Direction, move.Score));
        }

        activePositions = newActivePositions;
    }
}

var endPositionValue = positionsWithScores
    .Where(p => p.Key.Position == endPosition)
    .Min(e => e.Value);

Console.WriteLine($"Best score: {endPositionValue}");

// Part 2  Idea - work backwards (with backwards and rotate 90 degrees clockwise and anticlockwise moves as before)
// but instead subtract that amount. If the min score equals that value, then it is optimal

var activeBackwardsPositions = positionsWithScores
    .Where(p => p.Key.Position == endPosition && p.Value == endPositionValue)
    .Select(p => (p.Key.Position, p.Key.Direction, Score: p.Value))
    .ToList();
var pointsOnOptimalPath = new HashSet<Point>
{
    endPosition
};
while (activeBackwardsPositions.Count > 0)
{
    var newActivePositions = new List<(Point Position, Point Direction, int Score)>();
    foreach (var position in activeBackwardsPositions)
    {
        List<(Point Position, Point Direction, int Score)> possibleMoves =
        [
            (position.Position - new Size(position.Direction), position.Direction, position.Score - 1),
            (position.Position, RotateClockwise(position.Direction), position.Score - 1000),
            (position.Position, RotateAntiClockwise(position.Direction), position.Score - 1000)
        ];
        
        foreach (var move in possibleMoves)
        {
            if (positionsWithScores.TryGetValue((move.Position, move.Direction), out var bestScoreSoFar))
            {
                if (move.Score == bestScoreSoFar)
                {
                    pointsOnOptimalPath.Add(move.Position);
                    newActivePositions.Add((move.Position, move.Direction, move.Score));
                }
            }
        }
    }
    
    activeBackwardsPositions = newActivePositions;
}

Console.WriteLine($"Squares along optimal path: {pointsOnOptimalPath.Count}");

static Point GetStartingPosition(char[,] grid)
{
    for (int x = 0; x < grid.GetLength(0); x++)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            if (grid[x, y] == 'S')
            {
                return new Point(x, y);
            }
        }
    }
    
    throw new Exception("Could not find starting position");
}

static Point GetEndPosition(char[,] grid)
{
    for (int x = 0; x < grid.GetLength(0); x++)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            if (grid[x, y] == 'E')
            {
                return new Point(x, y);
            }
        }
    }
    
    throw new Exception("Could not find starting position");
}

static Point RotateAntiClockwise(Point positionDirection)
{
    if (positionDirection == new Point(1, 0))
    {
        return new Point(0, -1);
    }
            
    if (positionDirection == new Point(0, -1))
    {
        return new Point(-1, 0);
    }

    if (positionDirection == new Point(-1, 0))
    {
        return new Point(0, 1);
    }

    if (positionDirection == new Point(0, 1))
    {
        return new Point(1, 0);
    }

    throw new ArgumentOutOfRangeException();
}

static Point RotateClockwise(Point positionDirection)
{
    if (positionDirection == new Point(1, 0))
    {
        return new Point(0, 1);
    }

    if (positionDirection == new Point(0, 1))
    {
        return new Point(-1, 0);
    }

    if (positionDirection == new Point(-1, 0))
    {
        return new Point(0, -1);
    }

    if (positionDirection == new Point(0, -1))
    {
        return new Point(1, 0);
    }

    throw new ArgumentOutOfRangeException();

}