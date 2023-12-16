namespace AoC2023.Days;

public static class Day9
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(9);
        var timedExecutor = new TimedExecutor();
        await timedExecutor.ExecuteTimed(() => PartOne(input)); //1581679977
        await timedExecutor.ExecuteTimed(() => PartTwo(input)); //889
    }

    private static async Task PartOne(string[] input)
    {
        var histories = new List<int>();
        
        foreach (var line in input)
        {
            var diffsArray = GetDiffs(line).ToArray();            
            for (int i = diffsArray.Length - 2; i >= 0; i--) 
            {
                var placeholder = diffsArray[i].Last() + diffsArray[i + 1].Last();
                if (i == 0)
                    histories.Add(placeholder);
                else
                    diffsArray[i].Add(placeholder);
            }
        }
        Console.WriteLine(histories.Sum());
    }


    public static async Task PartTwo(string[] input)
    {
        var histories = new List<int>();

        foreach (var line in input)
        {
            var diffsArray = GetDiffs(line).ToArray();
            for (int i = diffsArray.Length - 2; i >= 0; i--)
            {
                var placeholder = diffsArray[i].First() - diffsArray[i + 1].First();
                if (i == 0)
                    histories.Add(placeholder);
                else
                    diffsArray[i].Insert(0, placeholder);
            }
        }
        Console.WriteLine(histories.Sum());
    }

    private static List<List<int>> GetDiffs(string line)
    {
        var diffs = new List<List<int>>();
        diffs.Add(line.Split(' ').Select(n => int.Parse(n)).ToList());
        while (true)
        {
            var splits = diffs.Last().ToArray();
            var lineDiffs = new List<int>();
            for (int i = 0; i < splits.Length - 1; i++)
            {
                lineDiffs.Add(splits[i + 1] - splits[i]);
            }
            diffs.Add(lineDiffs);
            if (lineDiffs.All(d => d == 0))
                break;
        }
        return diffs;
    }
}
