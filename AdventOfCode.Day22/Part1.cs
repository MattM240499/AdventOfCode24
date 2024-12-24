namespace AdventOfCode.Day22;

public class Part1
{
    public static void Run(ulong[] buyerStartSecretValues)
    {
        ulong sum = 0;
        foreach (var buyerStartSecret in buyerStartSecretValues)
        {
            var secretValue = CalculateSecretValue(buyerStartSecret, 2000);
            sum += secretValue;
        }

        Console.WriteLine($"Final sum: {sum}");
    }
    
    private static ulong CalculateSecretValue(ulong startSecret, uint generations)
    {
        var secret = startSecret;
        for (uint i = 0; i < generations; i++)
        {
            secret = Shared.GenerateNextSecret(secret);
        }

        return secret;
    }
}