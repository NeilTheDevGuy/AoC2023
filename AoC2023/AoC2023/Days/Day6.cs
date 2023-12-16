namespace AoC2023.Days;

public static class Day6
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(6);
        var timedExecutor = new TimedExecutor();
        await timedExecutor.ExecuteTimed(() => PartOne(input)); //281600
        await timedExecutor.ExecuteTimed(() => PartTwo(input)); //33875953
    }

    private static async Task PartOne(string[] input)
    {
        var times = input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().Select(t => int.Parse(t)).ToArray();
        var distances = input[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().Select(d => int.Parse(d)).ToArray();

        var winningDistances = new List<int>();

        for (int i = 0; i < times.Length; i++) 
        {
            var time = times[i];
            var distance = distances[i];
            var thisRaceWinningDistances = 0;
            for (int ms = 1; ms <= time; ms++)
            {
                var travelled = ms * (time - ms);
                if (travelled > distance)
                    thisRaceWinningDistances++;                    
            }
            winningDistances.Add(thisRaceWinningDistances);
        }

        var result = winningDistances.Aggregate(1, (x, y) => x * y);
        Console.WriteLine(result);
    }

    private static async Task PartTwo(string[] input) 
    {
        var time = long.Parse(input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().Aggregate("", (times, t) => times + t));
        var distance = long.Parse(input[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().Aggregate("", (times, t) => times + t));

        var winningDistances = new List<int>();
                
        var thisRaceWinningDistances = 0;
        for (long ms = 1; ms <= time; ms++)
        {
            var travelled = ms * (time - ms);
            if (travelled > distance)
                thisRaceWinningDistances++;
        }
        winningDistances.Add(thisRaceWinningDistances);

        var result = winningDistances.Aggregate(1, (x, y) => x * y);
        Console.WriteLine(result);
    }
}
