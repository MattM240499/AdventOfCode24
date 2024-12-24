namespace AdventOfCode.Day22;

public class Part2
{
    public static void Run(ulong[] buyerStartSecretValues)
    {
        var allMonkeyProfits = new Dictionary<PriceChangeSequence, int>();
        foreach (var buyerStartSecret in buyerStartSecretValues)
        {
            var bestPriceChangeSequences = GetPriceChangeSequenceProfits(buyerStartSecret, 2000);
            foreach (var (key, value) in bestPriceChangeSequences)
            {
                if (allMonkeyProfits.TryGetValue(key, out var profits))
                {
                    profits += value;
                }
                else
                {
                    profits = value;
                }

                allMonkeyProfits[key] = profits;
            }
        }

        var bestSequence = allMonkeyProfits
            .MaxBy(a => a.Value);

        Console.WriteLine($"Can achieve {bestSequence.Value} bananas with sequence: {bestSequence.Key}");
    }

    private static Dictionary<PriceChangeSequence, int> GetPriceChangeSequenceProfits(ulong buyerStartSecret, uint generations)
    {
        var secret = buyerStartSecret;
        var secretPrices = new int[generations + 1];
        secretPrices[0] = Convert.ToInt32(secret % 10);
        for (var i = 1; i < generations + 1; i++)
        {
            secret = Shared.GenerateNextSecret(secret);
            secretPrices[i] = Convert.ToInt32(secret % 10);
        }

        var priceChangeSequenceProfits = new Dictionary<PriceChangeSequence, int>();
        for (int i = 0; i < generations - 3; i++)
        {
            var priceChangeSequence = new PriceChangeSequence(
                secretPrices[i + 1] - secretPrices[i], 
                secretPrices[i + 2] - secretPrices[i + 1],
                secretPrices[i + 3] - secretPrices[i + 2],
                secretPrices[i + 4] - secretPrices[i + 3]);

            priceChangeSequenceProfits.TryAdd(priceChangeSequence, secretPrices[i + 4]);
        }

        return priceChangeSequenceProfits;
    }
}

public record struct PriceChangeSequence(int One, int Two, int Three, int Four);