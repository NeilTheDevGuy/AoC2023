namespace AoC2023.Utils;

public class Maths
{
    public static long CalculateGreatestCommonFactor(long a, long b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }
        
        return a;
    }

    public static long CalculateLeastCommonMultiple(long a, long b)
    {
        var gcf = CalculateGreatestCommonFactor(a, b);
        return gcf == 0 ? 0 : a / gcf * b;
    }

    public static long CalculateLeastCommonMultiple(long[] input)
    {
        long result = 1;

        for (int i = 0; i < input.Length; i++)
        {            
            result = CalculateLeastCommonMultiple(result, input[i]);
        }

        return result;
    }
}
