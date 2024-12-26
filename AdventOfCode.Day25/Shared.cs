namespace AdventOfCode.Day25;

public class Shared
{
    public static (List<List<int>> Locks, List<List<int>> Keys) ParseInput(string[] inputLines)
    {
        List<List<int>> keys = new List<List<int>>();
        List<List<int>> locks = new List<List<int>>();

        var text = string.Join(Environment.NewLine, inputLines);
        var parts = text.Split(Environment.NewLine + Environment.NewLine);

        foreach (var part in parts)
        {
            var lines = part.Split(Environment.NewLine);
            var isKey = lines[0][0] != '#';

            var start = 1;
            var end = lines.Length;
            var increment = 1;
            if (isKey)
            {
                start = lines.Length - 2;
                end = -1;
                increment = -1;
            }

            var lengths = CreateEmptyListOfLength(lines[0].Length);
            for (int i = start; i != end; i += increment)
            {
                var line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    if (line[j] == '#')
                    {
                        lengths[j]++;
                    }
                }
            }

            if (isKey)
            {
                keys.Add(lengths);
            }
            else
            {
                locks.Add(lengths);
            }
        }
        return (locks, keys);
    }

    private static List<int> CreateEmptyListOfLength(int length)
    {
        return Enumerable.Range(0, length).Select(i => 0).ToList();
    }
}