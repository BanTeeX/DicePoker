namespace DicePoker.Players;

internal class ConsolePlayer : BasePlayer
{
    public override byte[] Reroll()
    {
        Console.Write("Player dices to reroll: ");

        var input = Console.ReadLine();

        if (input is null)
        {
            return [];
        }

        var entries = input.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        byte[] reroll = entries.Select(byte.Parse).ToArray();

        _diceSet.Reroll(reroll);

        return reroll;
    }
}
