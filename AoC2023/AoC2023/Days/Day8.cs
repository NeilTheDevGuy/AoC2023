namespace AoC2023.Days;

public static class Day8
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(8);
        var timedExecutor = new TimedExecutor();
        await timedExecutor.ExecuteTimed(() => PartOne(input)); //12643
        await timedExecutor.ExecuteTimed(() => PartTwo(input)); //13133452426987
    }

    private static async Task PartOne(string[] input)
    {
        var instructions = input[0];
        var directions = GetDirections(input);

        var currentNode = "AAA";
        var instructionPos = 0;
        var steps = 0;        
        while (currentNode != "ZZZ")
        {
            if (instructionPos == instructions.Length) instructionPos = 0;                    
            var currentInstruction = instructions[instructionPos];
            var lr = directions[currentNode];
            currentNode = currentInstruction == 'L' ? lr.Item1 : lr.Item2;
            steps++;
            instructionPos++;
        }

        Console.WriteLine(steps);
    }

    public static async Task PartTwo(string[] input)
    {
        
        string instructions = input[0];
        var directions = GetDirections(input);

        var completed = new Dictionary<long, long>();
        var nodes = directions.Keys.Where(k => k.EndsWith('A')).ToArray();
        var instructionPos = 0;
        long steps = 0;

        while (completed.Count < nodes.Length)
        {
            for (var i = 0; i < nodes.Length; i++)
            {
                if (completed.ContainsKey(i)) continue;
                if (instructionPos == instructions.Length)
                    instructionPos = 0;
                
                var currentInstruction = instructions[instructionPos];
                var lr = directions[nodes[i]];
                nodes[i] = currentInstruction == 'L' ? lr.Item1 : lr.Item2;
                if (nodes[i].EndsWith('Z'))
                    completed[i] = steps + 1;
            }
            steps++;
            instructionPos++;
        };

        var solved = Maths.CalculateLeastCommonMultiple(completed.Values.ToArray());
        Console.WriteLine(solved); 
    }


    private static Dictionary<string, (string, string)> GetDirections(string[] input)
    {
        var directions = new Dictionary<string, (string, string)>();
        foreach (var line in input.Skip(2))
        {
            var split = line.Split('=');
            var dir = split[0].Trim();
            var left = split[1].Split(',')[0].Replace("(", "").Trim();
            var right = split[1].Split(',')[1].Replace(")", "").Trim();
            directions.Add(dir, (left, right));
        }
        return directions;
    }
}
