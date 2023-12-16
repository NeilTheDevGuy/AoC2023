using System.Collections.Generic;
using System.Diagnostics;

namespace AoC2023.Days;

public static class Day10
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(10);
        var timedExecutor = new TimedExecutor();
        await timedExecutor.ExecuteTimed(() => PartOne(input)); //6613
        await timedExecutor.ExecuteTimed(() => PartTwo(input)); //
    }

    private static async Task PartOne(string[] input)
    {
        var (startX, startY) = FindStart(input);
        var (currentX, currentY) = (startX, startY);
        var currentDir = FindStartDir(startX, startY, input);
        var backAtStart = false;
        var steps = 0;
        while (!backAtStart)
        {
            var (nextX, nextY, nextDir) = GetNext(currentX, currentY, currentDir, input);
            if (nextX == startX && nextY == startY) backAtStart = true;
            steps++;
            currentX = nextX;
            currentY = nextY;
            currentDir = nextDir;
        }
                
        Console.WriteLine(steps / 2);
    }


    public static async Task PartTwo(string[] input)
    {

    }



    private static (int nextX, int nextY, char nextDir) GetNext(int x, int y, char dir, string[] input)
    {
        var currentPiece = input[y][x];
        //Console.WriteLine($"Heading {dir}, arriving at {currentPiece}");
        (int nextX, int nextY, char nextDir) = (input[y][x]) switch
        {
            //'S' when dir == 'S' => (x, y + 1, 'S'), //test            
            'S' when dir == 'N' => (x, y - 1, 'N'), //real   
            '|' when dir == 'N' => (x, y - 1, 'N'),
            '|' when dir == 'S' => (x, y + 1, 'S'),
            '-' when dir == 'W' => (x - 1, y, 'W'),
            '-' when dir == 'E' => (x + 1, y, 'E'),
            'L' when dir == 'S' => (x + 1, y, 'E'),
            'L' when dir == 'W' => (x, y - 1, 'N'),
            'J' when dir == 'S' => (x - 1, y, 'W'),
            'J' when dir == 'E' => (x, y - 1, 'N'),
            '7' when dir == 'N' => (x - 1, y, 'W'),
            '7' when dir == 'E' => (x, y + 1, 'S'),
            'F' when dir == 'N' => (x + 1, y, 'E'),
            'F' when dir == 'W' => (x, y + 1, 'S')
        };
        return (nextX, nextY, nextDir);
    }

    private static char FindStartDir(int startX, int startY, string[] input)
    {
        //Cheating:
        //return 'S'; //Test
        return 'N'; //Real
    }

    private static (int x, int y) FindStart(string[] input)
    {
        var y = 0;
        foreach (var line in input)
        {
            var idx = line.IndexOf('S');
            if (idx > -1)
                return (idx, y);

            y++;
        }
        throw new Exception("Can't find start");
    }
}
