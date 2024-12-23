// Example
// var inputFile = "example.txt";

var inputFile = "input.txt";

var lines = File.ReadLines(inputFile).ToArray();

var towelPatterns = lines[0].Split(", ");

var designs = lines.Skip(2).ToArray();

// Part 1
var possibleDesigns = new List<string>();
foreach (var design in designs)
{
    if (IsPossible(design, towelPatterns))
    {
        possibleDesigns.Add(design);
    }
}

Console.WriteLine($"Possible designs: {possibleDesigns.Count}");

// Part 2

long numberOfPossibleDesigns = 0;
foreach (var design in designs)
{
    var numberOfArrangementsForDesign = GetNumberOfPossibleDesigns(design, towelPatterns);
    numberOfPossibleDesigns += numberOfArrangementsForDesign;
}

Console.WriteLine($"Number of possible designs: {numberOfPossibleDesigns}");


static long GetNumberOfPossibleDesigns(ReadOnlySpan<char> design, string[] towelPatterns)
{
    var solutionsPerIndex = new long[design.Length + 1];
    solutionsPerIndex[0] = 1;
    for (int i = 0; i < design.Length; i++)
    {
        var solutionsForIndex = solutionsPerIndex[i];
        if (solutionsForIndex == 0)
        {
            continue;
        }

        foreach (var pattern in towelPatterns)
        {
            if (design.Length < i + pattern.Length)
            {
                continue;
            }

            var designSpan = design[i..(i + pattern.Length)];
            if (MemoryExtensions.Equals(pattern, designSpan, StringComparison.Ordinal))
            {
                solutionsPerIndex[i + pattern.Length] += solutionsForIndex;
            }
        }
    }

    return solutionsPerIndex[design.Length];
}

static bool IsPossible(ReadOnlySpan<char> design, string[] towelPatterns)
{
    foreach (var pattern in towelPatterns)
    {
        if (design.Length < pattern.Length)
        {
            continue;
        }

        if (MemoryExtensions.Equals(pattern, design[..pattern.Length], StringComparison.Ordinal))
        {
            if (pattern.Length == design.Length)
            {
                return true;
            }

            if (IsPossible(design[pattern.Length..], towelPatterns))
            {
                return true;
            }
        }
    }

    return false;
}