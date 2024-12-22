using System.Numerics;

public class Computer
{
    public BigInteger RegistryA { get; set; }
    public BigInteger RegistryB { get; set; }
    public BigInteger RegistryC { get; set; }
    
    public long[] Instructions { get; set; }
    
    public long Pointer { get; set; }
    public List<long> Output { get; } = [];

    public void SetInitialState(BigInteger registryA, BigInteger registryB, BigInteger registryC, long[] instructions)
    {
        RegistryA = registryA;
        RegistryB = registryB;
        RegistryC = registryC;
        Instructions = instructions;
    }

    public bool Tick()
    {
        if (Pointer == Instructions.Length)
        {
            return false;
        }

        var opcode = Instructions[Pointer];
        var operand = Instructions[Pointer + 1];

        switch (opcode)
        {
            case 0:
            {
                // adv
                var value = SafeDivide(RegistryA, GetComboOperand(operand));
                RegistryA = value;
                Pointer += 2;
                break;
            }
            case 1:
            {
                // bxl
                var value = RegistryB ^ operand;
                RegistryB = value;
                Pointer += 2;
                break;
            }
            case 2:
            {
                // bst
                var value = GetComboOperand(operand) % 8;
                RegistryB = value;
                Pointer += 2;
                break;
            }
            //jnz
            case 3 when RegistryA == 0:
                Pointer += 2;
                break;
            case 3:
                Pointer = operand;
                break;
            case 4:
            {
                //bxc
                var value = RegistryB ^ RegistryC;
                RegistryB = value;
                Pointer += 2;
                break;
            }
            case 5:
            {
                // out
                var value = GetComboOperand(operand) % 8;
                Output.Add((long)value);
                Pointer += 2;
                break;
            }
            case 6:
            {
                //bdv
                var value = SafeDivide(RegistryA, GetComboOperand(operand));
                RegistryB = value;
                Pointer += 2;
                break;
            }
            case 7:
            {
                //bdv
                var value = SafeDivide(RegistryA, GetComboOperand(operand));
                RegistryC = value;
                Pointer += 2;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(opcode));
        }

        return true;
    }

    private static BigInteger SafeDivide(BigInteger numerator, BigInteger denominatorAsPowerOfTwo)
    {
        var numeratorPowerOfTwo = HighestPowerOfTwo(numerator);
        if (numeratorPowerOfTwo < denominatorAsPowerOfTwo)
        {
            return 0;
        }

        return numerator / BigInteger.Pow(2, (int)denominatorAsPowerOfTwo);
    }

    private BigInteger GetComboOperand(long operand)
    {
        return operand switch
        {
            <= 3 => operand,
            4 => RegistryA,
            5 => RegistryB,
            6 => RegistryC,
            7 => throw new ArgumentOutOfRangeException(nameof(operand)),
            _ => throw new ArgumentException($"Invalid operands {operand}")
        };
    }
    
    private static BigInteger HighestPowerOfTwo(BigInteger n)
    {
        BigInteger power = 0;
        BigInteger value = 1;
        while (true)
        {
            if (n < value)
            {
                break;
            }

            value *= 2;
            power++;
        }

        return power;
    }

    public void Clear()
    {
        Output.Clear();
        Pointer = 0;
    }
}