namespace DicePoker.Players;

internal class RandomPlayer : BasePlayer
{
    private readonly Random _random = new();

    public override byte[] Reroll()
    {
        List<byte> rerollList = [];

        List<int> indexes = Enumerable.Range(0, Dices.Length).ToList();

        var numDices = _random.Next(Dices.Length);
        for (int i = 0; i < numDices; i++)
        {
            var index = indexes[_random.Next(indexes.Count)];
            rerollList.Add(Dices[index]);
            indexes.Remove(index);
        }

        var reroll = rerollList.ToArray();

        _diceSet.Reroll(reroll);

        return reroll;
    }
}
