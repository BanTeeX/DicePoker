using DicePoker.Players;
using DicePoker.QTables;
using System.Diagnostics;

namespace DicePoker;

public class AITeacher(
    bool generateQTable = true,
    string path = "QTable.json",
    double learningRate = 0.1,
    double explorationRate = 0.1)
{
    private readonly QTable _qTable = new(generateQTable, path, learningRate, explorationRate);

    public void SaveQTable()
    {
        _qTable.SaveInFile();
    }

    public void Learn(int iterations)
    {
        Console.Write("""
            Learning in progress
            [____________________]
            """);

        int completed = 0;

        var stopwatch = Stopwatch.StartNew();
        Parallel.For(0, iterations, i =>
        {
            var aiPlayer = new AIPlayer(_qTable);

            aiPlayer.Roll();
            aiPlayer.Reroll();
            aiPlayer.Learn();

            completed++;

            if (completed % (iterations / 10) == 0)
            {
                UpdateProgress(completed / (iterations / 10));
            }
        });
        stopwatch.Stop();

        Console.Clear();

        Console.WriteLine($"Learning completed in {stopwatch.Elapsed.TotalSeconds:0} seconds");
    }

    public void CheckAgaistRandom(int iterations)
    {
        Console.Write("""
            Testing in progress
            [____________________]
            """);

        int winner1 = 0;
        int winner2 = 0;

        int completed = 0;

        Parallel.For(0, iterations, _ =>
        {
            var randomPlayer = new RandomPlayer();
            var aiPlayer = new AIPlayer(_qTable);

            randomPlayer.Roll();
            aiPlayer.Roll();

            randomPlayer.Reroll();
            aiPlayer.Reroll();

            var result1 = randomPlayer.EvaluateSet();
            var result2 = aiPlayer.EvaluateSet();

            if (result1 > result2)
            {
                winner1++;
            }
            else if (result1 < result2)
            {
                winner2++;
            }

            completed++;

            if (completed % (iterations / 10) == 0)
            {
                UpdateProgress(completed / (iterations / 10));
            }
        });

        Console.Clear();

        DisplayWinRates((double)winner1 / iterations, (double)winner2 / iterations);
    }

    private static void UpdateProgress(int progress)
    {
        Console.CursorLeft = progress * 2 - 1;
        Console.Write("##");
    }

    private static void DisplayWinRates(double random, double ai)
    {
        Console.WriteLine($"Win rates: Random {random:p2} AI {ai:p2}");
    }
}
