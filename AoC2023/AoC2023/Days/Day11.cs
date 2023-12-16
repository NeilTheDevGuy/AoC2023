namespace AoC2023.Days;

public static class Day11
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(11);
        var timedExecutor = new TimedExecutor();
        await timedExecutor.ExecuteTimed(() => PartOne(input)); // 9681886
        await timedExecutor.ExecuteTimed(() => PartTwo(input)); //
    }

    private static async Task PartOne(string[] input)
    {
        var expanded = Expand(input);        
        var galaxies = new Dictionary<int, (int, int)>();
        var galaxyCount = 0;
        for (int y = 0; y < expanded.Length; y++)
        {
            for (int x = 0; x < expanded[0].Length; x++)
            {
                if (expanded[y][x] == '#')
                {
                    galaxies.Add(++galaxyCount, (x, y));                                        
                }
            }
        }

        var pairs = new Dictionary<(int,int), int>();
        foreach (var galaxy in galaxies.Keys)
        {
            var otherGalaxies = galaxies.Keys.Where(k => k != galaxy);
            foreach (var otherGalaxy in otherGalaxies)
            {
                var distance = GetDistance(galaxies[galaxy].Item1, galaxies[galaxy].Item2, galaxies[otherGalaxy].Item1, galaxies[otherGalaxy].Item2);
                var pair1 = galaxy;
                var pair2 = otherGalaxy;
                if (otherGalaxy < galaxy)
                {
                    pair1 = otherGalaxy;
                    pair2 = galaxy;
                }
                if (!pairs.ContainsKey((pair1, pair2)))
                {
                    pairs.Add((pair1, pair2), distance);
                }
            }
        }

        Console.WriteLine(pairs.Values.Sum());
    }


    public static async Task PartTwo(string[] input)
    {

    }

    private static int GetDistance(int x, int y, int otherX, int otherY)
    {
        return Math.Abs(x - otherX) +  Math.Abs(y - otherY);
    }

    private static string[] Expand(string[] input)
    {
        var lst = input.ToList();
        var length = input.Length;
        var startOfEmpty = 0;
        var emptyLine = "";
;
        //Expand down
        for (int i = 0; i < length; i++)
        {
            var line = lst[i];            
            if (line.All(l => l == '.'))
            {
                if (startOfEmpty == 0) startOfEmpty = i;
                emptyLine = line;                
            }
            else
            {
                if (startOfEmpty > 0)
                {
                    var toInsert = i - startOfEmpty;
                    for (int z = 1; z <= toInsert; z++)
                    {
                        lst.Insert(z + startOfEmpty, emptyLine);
                    }
                    length += toInsert;
                    i += toInsert;
                    startOfEmpty = 0;
                }
            }
        }

        //Then across
        length = lst[0].Length;
        startOfEmpty = 0;
        for (int x = 0; x < length; x++)
        {
            var empty = true;
            
            for (int y = 0; y < lst.Count; y++)
            {
                var point = lst[y][x];
                if (point != '.')
                {
                    empty = false;
                    if (startOfEmpty == 0)
                        break;

                    var toInsert = x - startOfEmpty;
                    for (var z = 1; z <= toInsert; z++)
                    {
                        for (int i = 0; i < lst.Count; i++)
                        {
                            var line = lst[i];
                            var expandedLine = line.Insert(x, ".");
                            lst[i] = expandedLine;
                        }
                    }
                    length += toInsert;
                    x += toInsert;
                    startOfEmpty = 0;
                }
            }

            if (empty == true && startOfEmpty == 0)
                startOfEmpty = x;
        }

        return lst.ToArray();
    }
}
