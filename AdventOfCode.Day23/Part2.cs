namespace AdventOfCode.Day23;

public class Part2
{
    public static void Run(string[] lines)
    {
        var (allUsers, adjacencyMatrix) = Shared.ParseInput(lines);

        var bestNetwork = new List<string>();

        for (int i = 0; i < allUsers.Length; i++)
        {
            var connectedUsers = new List<string>();
            for (int j = i + 1; j < allUsers.Length; j++)
            {
                if (adjacencyMatrix[(allUsers[i], allUsers[j])])
                {
                    connectedUsers.Add(allUsers[j]);
                }
            }

            var network = FindBiggestNetworkContainingUser(allUsers[i], connectedUsers, adjacencyMatrix);
            if (network.Count > bestNetwork.Count)
            {
                bestNetwork = network;
            }
        }

        var password = string.Join(",", bestNetwork.OrderBy(b => b));

        Console.WriteLine($"Lan party password: {password}");
    }

    private static List<string> FindBiggestNetworkContainingUser(string user, List<string> connectedUsers, Dictionary<(string, string), bool> adjacencyMatrix)
    {
        if (connectedUsers.Count == 0)
        {
            return [user];
        }
        
        var bestNetwork = new List<string>();
        var network = new List<string>(connectedUsers.Count) { user, connectedUsers[0] };
        var connectedUserIndex = new List<int>(connectedUsers.Count) { 0 };
        while (true)
        {
            var currentIndex = connectedUserIndex[^1];
            // Get the last added user
            var lastUser = connectedUsers[currentIndex];
            // Check the network is still valid
            bool isValid = true;
            // Ignore 1st (current user which we know is definitely connected), and the last (user we are comparing to) 
            for (int i = 1; i < network.Count - 1; i++)
            {
                if (!adjacencyMatrix[(network[i], lastUser)])
                {
                    isValid = false;
                    break;
                }
            }
            
            var newUserIndex = currentIndex + 1;
            bool isFinalUser = (newUserIndex == connectedUsers.Count);
            
            if (isValid)
            {
                if (network.Count > bestNetwork.Count)
                {
                    bestNetwork = network.ToList();
                }

                if (isFinalUser)
                {
                    if (connectedUserIndex.Count == 1)
                    {
                        break;
                    }
                    network.RemoveAt(network.Count - 1);
                    connectedUserIndex.RemoveAt(connectedUserIndex.Count - 1);
                    connectedUserIndex[^1]++;
                }
                else
                {
                    network.Add(connectedUsers[newUserIndex]);
                    connectedUserIndex.Add(newUserIndex);
                }
            }
            if(!isValid)
            {
                // Network not valid, iterate to the next network
                if(isFinalUser)
                {
                    if (connectedUserIndex.Count == 1)
                    {
                        // I.e. we have 0 connected to the final user only.
                        break;
                    }
                    network.RemoveAt(network.Count - 1);
                    connectedUserIndex.RemoveAt(connectedUserIndex.Count - 1);
                }

                connectedUserIndex[^1]++;
            }
        }

        return bestNetwork;
    }
}