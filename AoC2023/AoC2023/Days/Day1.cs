using System.Text.RegularExpressions;

namespace AoC2023.Days;

public static class Day1
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(1);
        var timedExecutor = new TimedExecutor();
        await timedExecutor.ExecuteTimed(() => PartOne(input)); //56397
        await timedExecutor.ExecuteTimed(() => PartTwo(input)); //55701
    }

    private static async Task PartOne(string[] input)
    {
        var numbers = new List<int>();
        foreach (var line in input)
        {
            var firstNumber = Regex.Match(line, @"\d").Value;
            var secondNumber = Regex.Match(line, @"\d", RegexOptions.RightToLeft).Value;
            numbers.Add(int.Parse(firstNumber + secondNumber));
        }
        Console.WriteLine(numbers.Sum());
    }

    private static async Task PartTwo(string[] input)
    {
        var numberStrings = new Dictionary<string, string>() { 
            { "one", "1" }, { "two", "2" }, { "three", "3" }, { "four", "4" }, { "five", "5" }, { "six", "6" }, { "seven", "7" }, { "eight", "8" }, { "nine", "9" },
            { "1", "1" }, { "2", "2" }, { "3", "3" }, { "4", "4" }, { "5", "5" }, { "6", "6" }, { "7", "7" }, { "8", "8"}, { "9", "9" }
        };

        var numbers = new List<int>();

        foreach (var line in input)
        {
            int firstStringLocation = int.MaxValue, lastStringLocation = int.MinValue;
            string firstNumber = "", lastNumber = "";            

            foreach (var number in numberStrings.Keys)
            {                
                var thisLocation = line.IndexOf(number);
                if (thisLocation > -1 && thisLocation < firstStringLocation)
                {
                    firstStringLocation = thisLocation;
                    firstNumber = numberStrings[number];
                }
                                
                thisLocation = line.LastIndexOf(number);
                if (thisLocation > -1 && thisLocation > lastStringLocation)
                {
                    lastStringLocation = thisLocation;
                    lastNumber = numberStrings[number];
                }
            }

            numbers.Add(int.Parse(firstNumber + lastNumber));
        }
        Console.WriteLine(numbers.Sum());
    }
}
