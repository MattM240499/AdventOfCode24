namespace AdventOfCode.Day25;

public class Part1
{
    public static void Run(string[] lines)
    {
        var input = Shared.ParseInput(lines);
        List<(List<int> Lock, List<int> Key)> combinations = [];
        foreach (var @lock in input.Locks)
        {
            foreach (var key in input.Keys)
            {
                var valid = true;
                for (int i = 0; i < key.Count; i++)
                {
                    if (key[i] + @lock[i] > 5)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    combinations.Add((@lock, key));
                }
            }
        }
        
        Console.WriteLine($"Combinations: {combinations.Count}");
    }
}