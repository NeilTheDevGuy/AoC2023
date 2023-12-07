namespace AoC2023.Days;

public static class Day7
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(7);
        var timedExecutor = new TimedExecutor();
        await timedExecutor.ExecuteTimed(() => Run(input, false)); //251058093
        await timedExecutor.ExecuteTimed(() => Run(input, true)); //249781879
    }

    private static async Task Run(string[] input, bool jacksAreJokers)
    {        
        var rankedHands = new LinkedList<Hand>();

        foreach (var line in input)
        {
            var splitHand = line.Split(' ');
            var hand = new Hand
            {
                Cards = splitHand[0],
                Winnings = int.Parse(splitHand[1]),
                Score = CalculateScore(splitHand[0], jacksAreJokers)
            };

            if (rankedHands.Count == 0)
            {
                rankedHands.AddFirst(hand);
                continue;
            }

            var haveAdded = false;
            foreach (LinkedListNode<Hand> node in rankedHands.Nodes())
            {
                if (!IsStronger(hand, node.Value, jacksAreJokers))
                {
                    rankedHands.AddBefore(node, new LinkedListNode<Hand>(hand));
                    haveAdded = true;
                    break;
                }
            }
            if (!haveAdded)
                rankedHands.AddLast(new LinkedListNode<Hand>(hand));
        }

        var rankedArray = rankedHands.ToArray();
        var winnings = 0;
        for (int i = 0; i < rankedArray.Length; i++)
        {
            winnings += rankedArray[i].Winnings * (i + 1);
        }

        Console.WriteLine(winnings);
    }

    private static bool IsStronger(Hand myHand, Hand rankedHand, bool jacksAreJokers)
    {
        if (myHand.Score > rankedHand.Score) return true;
        if (myHand.Score < rankedHand.Score) return false;
        for (int i = 0; i < myHand.Cards.Length; i++)
        {
            var myCard = myHand.Cards[i];
            var rankedCard = rankedHand.Cards[i];
            if (myCard == rankedCard) continue;

            return MapCardToValue(myCard, jacksAreJokers) > MapCardToValue(rankedCard, jacksAreJokers);
        }
        return false;
    }

    private static int MapCardToValue(char card, bool jacksAreJokers)
    {
        return card switch
        {
            'J' when jacksAreJokers => 1,
            'J' when !jacksAreJokers => 11,
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'T' => 10,
            'Q' => 12,
            'K' => 13,
            'A' => 14
        };
    }

    private static int CalculateScore(string hand, bool jacksAreJokers)
    {
        return jacksAreJokers ? CalculateScoreWithJokers(hand) : CalculateScoreWithoutJokers(hand);
    }

    private static int CalculateScoreWithoutJokers(string hand)
    {
        var groupings = hand            
            .GroupBy(c => c)
            .Select(s => s.Count())
            .OrderByDescending(o => o)
            .ToArray();

        if (groupings[0] == 5) return 7; //5 of a kind
        if (groupings[0] == 4) return 6; //4 of a kind
        if (groupings[0] == 3 && groupings[1] == 2) return 5; //full house
        if (groupings[0] == 3 && groupings[1] == 1) return 4; //3 of a kind        
        if (groupings[0] == 2 && groupings[1] == 2) return 3; //2 pair        
        if (groupings[0] == 2 && groupings[1] == 1) return 2; //1 pair
        if (groupings[0] == 1) return 1; //high card

        throw new Exception("invalid hand");
    }

    private static int CalculateScoreWithJokers(string hand)
    {
        var jokers = hand.Count(j => j == 'J');
        if (jokers == 5) return 7; //edge case for JJJJJ

        var groupings = hand
            .Where(j => j != 'J')
            .GroupBy(c => c)
            .Select(s => s.Count())
            .OrderByDescending(o => o)
            .ToArray();

        if (groupings[0] + jokers == 5) return 7; //5 of a kind
        if (groupings[0] + jokers == 4) return 6; //4 of a kind
        if (groupings[0] + jokers == 3 && groupings[1] == 2) return 5; //full house
        if (groupings[0] + jokers == 3 && groupings[1] == 1) return 4; //3 of a kind        
        if (groupings[0] + jokers == 2 && groupings[1] == 2) return 3; //2 pair        
        if (groupings[0] + jokers == 2 && groupings[1] == 1) return 2; //1 pair
        if (groupings[0] + jokers == 1) return 1; //high card

        throw new Exception("invalid hand");
    }

    private class Hand
    {
        public string Cards;
        public int Winnings;
        public int Score;
    }

}
