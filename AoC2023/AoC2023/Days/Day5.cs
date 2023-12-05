namespace AoC2023.Days;

public static class Day5
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(5);
        var timedExecutor = new TimedExecutor();
        await timedExecutor.ExecuteTimed(() => PartOne(input)); //323142486
        await timedExecutor.ExecuteTimed(() => PartTwo(input)); //79874951
    }

    private static async Task PartOne(string[] input)
    {
        var seedNumbers = input[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var seeds = seedNumbers.Select(s => long.Parse(s)).ToList();

        var maps = GetMappings(input);

        var minVal = long.MaxValue;
        foreach (var seed in seeds)
        {
            var mapValue = seed;
            foreach (var map in maps)
            {
                foreach (var mapping in map)
                {
                    if (mapValue >= mapping.Item1 && mapValue <= mapping.Item2)
                    {
                        mapValue += mapping.Item3;
                        break;
                    }
                }
            }
            if (mapValue < minVal) minVal = mapValue;
        }

        Console.WriteLine(minVal);
    }

    private static async Task PartTwo(string[] input) 
    {
        var seedNumbers = input[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var seeds = seedNumbers.Select(s => long.Parse(s)).ToList();

        var maps = GetMappings(input);
                
        var seedRanges = new List<(long, long)>(); //Start, end

        for (int i = 0; i < seeds.Count; i++)
        {
            seedRanges.Add((seeds[i], seeds[i] + seeds[++i] - 1));
        }

        foreach (var map in maps)
        {
            var orderedMap = map.OrderBy(m => m.Item1).ToList();

            var adjustedRanges = new List<(long, long)>();
            foreach (var range in seedRanges)
            {
                var seedRange = range;
                foreach (var thisOrderedMap in orderedMap)
                {
                    if (seedRange.Item1 < thisOrderedMap.Item1)
                    {
                        var toAdd = Math.Min(seedRange.Item2, thisOrderedMap.Item1 - 1);
                        adjustedRanges.Add((seedRange.Item1, toAdd));
                        seedRange.Item1 = thisOrderedMap.Item1;
                        if (seedRange.Item1 > seedRange.Item2)
                            break;
                    }

                    if (seedRange.Item1 <= thisOrderedMap.Item2)
                    {
                        var toAdd = Math.Min(seedRange.Item2, thisOrderedMap.Item2) + thisOrderedMap.Item3;
                        adjustedRanges.Add((seedRange.Item1 + thisOrderedMap.Item3, toAdd));
                        seedRange.Item1 = thisOrderedMap.Item2 + 1;
                        if (seedRange.Item1 > seedRange.Item2)
                            break;
                    }
                }
                if (seedRange.Item1 <= seedRange.Item2)
                    adjustedRanges.Add(seedRange);
            }
            seedRanges = adjustedRanges;
        }

        var minVal = seedRanges.Min(r => r.Item1);
        Console.WriteLine(minVal);        
    }

    private static List<List<(long, long, long)>> GetMappings(string[] input)
    {
        var maps = new List<List<(long, long, long)>>(); //Start, End, Offset
        List<(long, long, long)> thisMap = new();

        foreach (var line in input.Skip(2))
        {
            if (line.Contains("map"))
            {
                thisMap = new List<(long, long, long)>();
                continue;
            }
            else if (string.IsNullOrEmpty(line))
            {
                maps.Add(thisMap);
                continue;
            }

            var start = long.Parse(line.Split(' ')[0]);
            var end = long.Parse(line.Split(' ')[1]);
            var offset = long.Parse(line.Split(' ')[2]);
            thisMap.Add((end, end + offset - 1, start - end));
        }
        maps.Add(thisMap);

        return maps;
    }
}
