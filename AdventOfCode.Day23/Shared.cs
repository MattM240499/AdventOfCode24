namespace AdventOfCode.Day23;

public class Shared
{
    public static (string[], Dictionary<(string, string), bool>) ParseInput(string[] lines)
    {
        var networkConnections = lines.Select(l =>
        {
            var parts = l.Split("-");
            return (User1: parts[0], User2: parts[1]);
        }).ToArray();

        var allUsers = networkConnections.Select(n => n.User1).Union(networkConnections.Select(n => n.User2)).ToArray();
        
        var adjacencyMatrix = new Dictionary<(string, string), bool>();
        
        foreach (var user1 in allUsers)
        {
            foreach (var user2 in allUsers)
            {
                adjacencyMatrix[(user1, user2)] = false;
            }
        }

        foreach (var connection in networkConnections)
        {
            adjacencyMatrix[(connection.User1, connection.User2)] = true;
            adjacencyMatrix[(connection.User2, connection.User1)] = true;
        }

        return (allUsers, adjacencyMatrix);
    }
}