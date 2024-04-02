namespace DicePoker.Players;

internal abstract class BasePlayer
{
    protected readonly DiceSet _diceSet = new();

    public byte[] Dices => _diceSet.Dices;

    public void Roll()
    {
        _diceSet.Roll();
    }

    public abstract byte[] Reroll();

    public double EvaluateSet()
    {
        return _diceSet.EvaluateSet();
    }
}
