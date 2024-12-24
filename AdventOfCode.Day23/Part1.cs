namespace AdventOfCode.Day23;

public class Part1
{
    public static void Run(string[] lines)
    {
        var (allUsers, adjacencyMatrix) = Shared.ParseInput(lines);

        var trios = new List<(string User1, string User2, string User3)>();

        for (int i = 0; i < allUsers.Length; i++)
        {
            for (int j = i + 1; j < allUsers.Length; j++)
            {
                if (!adjacencyMatrix[(allUsers[i], allUsers[j])])
                {
                    continue;
                }

                for (int k = j + 1; k < allUsers.Length; k++)
                {
                    if (!adjacencyMatrix[(allUsers[i], allUsers[k])] || !adjacencyMatrix[(allUsers[j], allUsers[k])])
                    {
                        continue;
                    }
                    
                    trios.Add((allUsers[i], allUsers[j], allUsers[k]));
                }
            }
        }

        var triosPotentiallyContainingTheChiefHistorian = trios.Where(t =>
            t.User1.StartsWith('t') ||
            t.User2.StartsWith('t') ||
            t.User3.StartsWith('t')).ToArray();

        Console.WriteLine($"Number of trios that may contain the chief historian: {triosPotentiallyContainingTheChiefHistorian.Length}");
        
    }
}