using DicePoker.Players;
using DicePoker.QTables;
using System.Text;

namespace DicePoker;

public class DicePokerGame(string path)
{
    public void Run()
    {
        var qTable = new QTable(false, path);

        var player = new ConsolePlayer();
        var aiPlayer = new AIPlayer(qTable, true);

        player.Roll();
        aiPlayer.Roll();

        DisplayDices(player.Dices, aiPlayer.Dices);
        Console.WriteLine();

        DisplayReroll(player.Reroll(), aiPlayer.Reroll());
        Console.WriteLine();

        DisplayDices(player.Dices, aiPlayer.Dices);
        Console.WriteLine();

        DisplayEvaluation(player.EvaluateSet(), aiPlayer.EvaluateSet());
        Console.WriteLine();

        aiPlayer.Learn();
    }

    private static void DisplayEvaluation(double player, double ai)
    {
        Console.WriteLine("Results:");
        Console.WriteLine($"Player: {player:f5} AI: {ai:f5}");

        Console.WriteLine();

        var msg = player == ai ? "Draw" : player > ai ? "Winner is player" : "Winner is AI";
        Console.WriteLine(msg);
    }

    private static void DisplayReroll(byte[] player, byte[] ai)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Dices reroll:");

        builder.Append("Player: ");
        foreach (var dice in player)
        {
            builder.Append(dice + " ");
        }

        builder.Append(" AI: ");
        foreach (var dice in ai)
        {
            builder.Append(dice + " ");
        }

        Console.WriteLine(builder);
    }

    private static void DisplayDices(byte[] player, byte[] ai)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Dices state:");

        builder.Append("Player: ");
        foreach (var dice in player)
        {
            builder.Append(dice + " ");
        }

        builder.Append(" AI: ");
        foreach (var dice in ai)
        {
            builder.Append(dice + " ");
        }

        Console.WriteLine(builder);
    }
}
