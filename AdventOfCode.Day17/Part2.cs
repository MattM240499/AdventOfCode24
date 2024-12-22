using System.Numerics;

namespace AdventOfCode.Day17;

public class Part2
{
    public static void Run(long[] program, long registerA, long registerB, long registerC)
    {
        //Part2_Naive(program, registerB, registerC);
        
        FindSolution(program);
    }

    /// <summary>
    /// Idea: In the input program, each iteration the number is divided by 8. Also, each iteration the program only depends on the value in Registry A
    /// This means that we can work backwards to work out for which inputs will produce each output
    /// Consider xn = ((...((a0 * 8) + a1) * 8) + a3) * 8 +... + an-1) * 8) + an))..), where xn is the value in Registry A of the program producing an output of length n
    /// and ai in {0, 7}
    /// We can iteratively find a0 ... an that output the program values, starting from a0.
    /// This will reduce the search space dramatically!
    /// </summary>
    private static void FindSolution(long[] program)
    {
        var requiredOutputs = program.Length;
        var currentIndex = 0;
        var values = new BigInteger[requiredOutputs];
        BigInteger value = new BigInteger(1);
        var computer = new Computer();
        while (currentIndex >= 0)
        {
            // Basically, if we've checked all values, go down a power of 8, and continue
            if (currentIndex != 0 && value / 8 != values[currentIndex - 1])
            {
                value = values[currentIndex - 1];
                value++;
                currentIndex--;
                continue;
            }
            
            values[currentIndex] = value;

            var expectedNumberOfOutputs = currentIndex + 1;
            
            computer.Clear();
            computer.SetInitialState(value, 0, 0, program);

            while (computer.Tick())
            {
            }

            var programIndex = program.Length - currentIndex - 1;
            var computerIndex = computer.Output.Count - currentIndex - 1;
            if (computer.Output.Count != expectedNumberOfOutputs)
            {
                throw new Exception("Something went quite wrong");
            }

            if (program[programIndex] == computer.Output[computerIndex])
            {
                if (currentIndex == requiredOutputs - 1)
                {
                    break;
                }

                value *= 8;
                currentIndex++;
            }
            else
            {
                value++;
            }
        }

        if (currentIndex == -1)
        {
            Console.WriteLine("Could not find a solution");
        }
        else
        {
            Console.WriteLine($"Solution: {value}");
        }
    }

    /// <summary>
    /// Will never finish, lols
    /// </summary>
    private static void Part2_Naive(long[] program, long registerB, long registerC)
    {
        BigInteger registryAMin = BigInteger.Pow(8, program.Length - 1);
        BigInteger registryAMax = BigInteger.Pow(8, program.Length);
        Console.WriteLine($"Searching for solution on the interval: {registryAMin} - {registryAMax}");
        var registryAStart = registryAMin;
        var computer = new Computer();
        while (registryAStart < registryAMax)
        {
            computer.Clear();
            computer.SetInitialState(registryAStart, registerB, registerC, program);
            var failed = false;
            var previousCount = 0;
            while (computer.Tick())
            {
                if (computer.Output.Count != previousCount)
                {
                    if (computer.Output[previousCount] != program[previousCount])
                    {
                        failed = true;
                        break;
                    }

                    previousCount++;
                }
            }

            if (!failed)
            {
                break;
            }

            
            registryAStart++;
        }

        if (registryAStart == registryAMax)
        {
            Console.WriteLine($"No start value for Registry A that produces program: {String.Join(",", program)}");
        }
        else
        {
            Console.WriteLine($"Registry A should be initialised to {registryAStart}");
        }
    }
}

public class InverseComputer()
{
    public void Instruction()
    {
        
    }
}