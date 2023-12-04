using System.Text.RegularExpressions;

namespace AoC2023.Days;

public static class Day4
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(4);
        await PartOne(input); //26914
        await PartTwo(input); //13080971
    }

    private static async Task PartOne(string[] input)
    {
        var overallPoints = 0;
        foreach (var line in input)
        {
            var leftAndRight = line.Split('|');
            var left = leftAndRight[0];
            var right = leftAndRight[1];

            var winningNumbers = left.Split(":")[1]
                                .Split(" ")
                                .Where(n => !string.IsNullOrWhiteSpace(n))
                                .Select(w => int.Parse(w.Trim()))
                                .ToList();

            var myNumbers = right.Split(" ")
                            .Where(w => !String.IsNullOrWhiteSpace(w))
                            .Select(w => int.Parse(w.Trim()))
                            .ToList();

            var matches = winningNumbers.Intersect(myNumbers).ToList();
            var points = 0;
            foreach (var match in matches)
            {
                if (points == 0) points = 1;
                else points *= 2;
            }
            overallPoints += points; 
        }
        Console.WriteLine(overallPoints);
    }

    private static async Task PartTwo(string[] input)
    {
        var cards = new Queue<string>();
        var winningCardsCount = 0;

        foreach (var line in input)
        {
            cards.Enqueue(line);
        }        
        
        while (cards.Count > 0)
        {
            var line = cards.Dequeue();
            var leftAndRight = line.Split('|');
            var left = leftAndRight[0];            
            var right = leftAndRight[1];

            var currentCard = int.Parse(left.Split(':')[0].Replace("Card", "").Trim());

            var winningNumbers = left.Split(":")[1]
                                .Split(" ")
                                .Where(n => !string.IsNullOrWhiteSpace(n))
                                .Select(w => int.Parse(w.Trim()))
                                .ToList();

            var myNumbers = right.Split(" ")
                            .Where(w => !String.IsNullOrWhiteSpace(w))
                            .Select(w => int.Parse(w.Trim()))
                            .ToList();

            var matches = winningNumbers.Intersect(myNumbers);
            winningCardsCount++;

            for (int i = 0; i < matches.Count(); i++)
            {
                cards.Enqueue(input[currentCard + i]);
            }            
        }        
        Console.WriteLine(winningCardsCount);
    }
}
