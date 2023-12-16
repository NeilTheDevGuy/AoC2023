using System.Text.RegularExpressions;

namespace AoC2023.Days;

public static class Day3
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(3);
        //await PartOne(input); //507214
        await PartTwo(input); //72553319
    }

    private static async Task PartOne(string[] input)
    {
        var partNumbers = new List<int>();
        for (var y = 0; y < input.Length; y++)
        {
            var line = input[y];
            var gotNumber = false;
            var currentNumber = "";
            for (var x = 0; x < line.Length; x++)
            {
                if (IsNumeric(line[x]))
                {
                    currentNumber += line[x];
                    gotNumber = true;
                }
                else
                {
                    if (gotNumber)
                    {
                        Console.WriteLine($"Found number {currentNumber}");
                        var numStartX = line.IndexOf(currentNumber, x - currentNumber.Length );
                        var numEndX = numStartX + currentNumber.Length - 1;

                        gotNumber = false;
                        if (NearPart(input, numStartX, numEndX, y))
                        {
                            Console.WriteLine($"Number {currentNumber} is near Part");
                            var number = int.Parse(currentNumber);
                            partNumbers.Add(number);
                        }
                        currentNumber = "";
                    }
                }
            }
            if (gotNumber)
            {
                Console.WriteLine($"Found number {currentNumber}");
                var numStartX = line.IndexOf(currentNumber);
                var numEndX = numStartX + currentNumber.Length - 1;

                gotNumber = false;
                if (NearPart(input, numStartX, numEndX, y))
                {
                    Console.WriteLine($"Number {currentNumber} is near Part");
                    var number = int.Parse(currentNumber);
                    partNumbers.Add(number);
                }
                currentNumber = "";
            }
        }
        Console.WriteLine(partNumbers.Sum());
    }

    private static async Task PartTwo(string[] input)
    {
        var gearLocations = new Dictionary<(int, int), List<int>>();
        for (var y = 0; y < input.Length; y++)
        {
            var line = input[y];
            var gotNumber = false;
            var currentNumber = "";
            for (var x = 0; x < line.Length; x++)
            {
                if (IsNumeric(line[x]))
                {
                    currentNumber += line[x];
                    gotNumber = true;
                }
                else
                {
                    if (gotNumber)
                    {
                        Console.WriteLine($"Found number {currentNumber}");
                        var numStartX = line.IndexOf(currentNumber, x - currentNumber.Length);
                        var numEndX = numStartX + currentNumber.Length - 1;

                        gotNumber = false;
                        (bool isNear, int gearX, int gearY) = NearGear(input, numStartX, numEndX, y);
                        if (isNear)
                        {
                            Console.WriteLine($"Number {currentNumber} is near the Gear at {gearX}, {gearY} ");
                            var number = int.Parse(currentNumber);
                            if (gearLocations.ContainsKey((gearX, gearY)))
                            {
                                gearLocations[(gearX, gearY)].Add(number);
                            }
                            else
                            {
                                gearLocations.Add((gearX, gearY), new List<int> { number });
                            }
                        }
                        currentNumber = "";
                    }
                }
            }
            if (gotNumber)
            {
                Console.WriteLine($"Found number {currentNumber}");
                var numStartX = line.IndexOf(currentNumber);
                var numEndX = numStartX + currentNumber.Length - 1;

                gotNumber = false;
                (bool isNear, int gearX, int gearY) = NearGear(input, numStartX, numEndX, y);
                if (isNear)
                {
                    Console.WriteLine($"Number {currentNumber} is near the Gear at {gearX}, {gearY} ");
                    var number = int.Parse(currentNumber);
                    if (gearLocations.ContainsKey((gearX, gearY)))
                    {
                        gearLocations[(gearX, gearY)].Add(number);
                    }
                    else
                    {
                        gearLocations.Add((gearX, gearY), new List<int> { number });
                    }
                }
                currentNumber = "";
            }
        }
        var gears = gearLocations.Where(g => g.Value.Count > 1);

        var gearTotal = 0;
        foreach (var gear in gears)
        {
            var lst = gear.Value;
            int ratio = lst.Aggregate(1, (x, y) => x * y);
            gearTotal += ratio;
        }

        Console.WriteLine(gearTotal);
    }

    private static bool NearPart(string[] input, int startX, int endX, int y)
    {
        //Check left
        if (startX - 1 >= 0)
            if (IsEnginePart(input[y][startX - 1])) return true;

        //Check right
        if (endX + 1 <= input[y].Length - 1)
            if (IsEnginePart(input[y][endX + 1])) return true;

        //Check any part above, below and diag
        var diagStart = startX - 1 < 0 ? 0 : startX - 1;
        var diagEnd = endX + 1 == input[y].Length ? input[y].Length - 1 : endX + 1;
        for (int adjX = diagStart; adjX <= diagEnd; adjX++)
        {
            if (y - 1 >= 0)
                if (IsEnginePart(input[y - 1][adjX])) return true; 

            if (y + 1 <= input.Length - 1)
                if (IsEnginePart(input[y + 1][adjX])) return true;
        }

        return false;
    }

    private static (bool isNear, int gearX, int gearY) NearGear(string[] input, int startX, int endX, int y)
    {
        //Check left
        if (startX - 1 >= 0)
            if (IsGearPart(input[y][startX - 1])) return (true, startX - 1, y);

        //Check right
        if (endX + 1 <= input[y].Length - 1)
            if (IsGearPart(input[y][endX + 1])) return (true, endX + 1, y);

        //Check any part above, below and diag
        var diagStart = startX - 1 < 0 ? 0 : startX - 1;
        var diagEnd = endX + 1 == input[y].Length ? input[y].Length - 1 : endX + 1;
        for (int adjX = diagStart; adjX <= diagEnd; adjX++)
        {
            if (y - 1 >= 0)
                if (IsGearPart(input[y - 1][adjX])) return (true, adjX, y - 1);

            if (y + 1 <= input.Length - 1)
                if (IsGearPart(input[y + 1][adjX])) return (true, adjX, y + 1);
        }

        return (false, -1, -1);
    }

    private static bool IsNumeric(char c)
    {
        var numbers = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        return numbers.Contains(c);
    }

    private static bool IsEnginePart(char c)
    {
        return !IsNumeric(c) && c != '.';
    }

    private static bool IsGearPart(char c)
    {
        return c == '*';
    }
}
