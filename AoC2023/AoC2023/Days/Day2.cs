using System.Text.RegularExpressions;

namespace AoC2023.Days;

public static class Day2
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(2);
        var timedExecutor = new TimedExecutor();
        await timedExecutor.ExecuteTimed(() => PartOne(input)); //2268
        await timedExecutor.ExecuteTimed(() => PartTwo(input)); //63542
    }

    private static async Task PartOne(string[] input)
    {
        int red = 12, green = 13, blue = 14;
        var possibles = new HashSet<int>();

        foreach (var line in input)
        {
            var split = line.Split(':');
            var gameId = int.Parse(split[0].Replace("Game", "").Trim());
            possibles.Add(gameId); //Assume possible and remove later if not
            var turns = split[1].Split(";");
            foreach (var turn in turns)
            {
                var cubes = turn.Split(',');
                foreach (var cube in cubes)
                {
                    var colour = cube.Trim().Split(' ')[1].Trim();
                    var count = int.Parse(cube.Trim().Split(' ')[0].Trim());
                    if (colour == "red" && count > red) possibles.Remove(gameId);
                    if (colour == "green" && count > green) possibles.Remove(gameId);
                    if (colour == "blue" && count > blue) possibles.Remove(gameId);
                }
            }
        }
        Console.WriteLine($"Sum - {possibles.Sum()}");
    }

    private static async Task PartTwo(string[] input)
    {
        var powers = new List<int>();

        foreach (var line in input)
        {
            var split = line.Split(':');
            var gameId = int.Parse(split[0].Replace("Game", "").Trim());
            var turns = split[1].Split(";");

            int minRed = 0, minBlue = 0, minGreen = 0;
            foreach (var turn in turns)
            {
                var cubes = turn.Split(',');
                foreach (var cube in cubes)
                {
                    var colour = cube.Trim().Split(' ')[1].Trim();
                    var count = int.Parse(cube.Trim().Split(' ')[0].Trim());
                    if (colour == "red" && count > minRed) minRed = count;
                    if (colour == "blue" && count > minBlue) minBlue = count;
                    if (colour == "green" && count > minGreen) minGreen = count;                    
                }
            }
            powers.Add(minRed * minBlue * minGreen);
        }

        Console.WriteLine($"Sum - {powers.Sum()}");
    }
}
