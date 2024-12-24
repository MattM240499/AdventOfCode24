namespace AdventOfCode.Day22;

public class Shared
{
    public static ulong GenerateNextSecret(ulong secret)
    {
        var secretTimes64 = secret * 64;
        secret = Mix(secret, secretTimes64);

        secret = Prune(secret);
    
        var secretDividedBy32 = secret / 32;
        secret = Mix(secret, secretDividedBy32);
        secret = Prune(secret);
    
        var secretTimes2024 = secret * 2048;
        secret = Mix(secret, secretTimes2024);
        secret = Prune(secret);

        return secret;
    }

    private static ulong Mix(ulong secret, ulong value)
    {
        return secret ^ value;
    }

    private static ulong Prune(ulong secret)
    {
        return secret % 16777216;
    }
}