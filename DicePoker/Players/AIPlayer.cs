using DicePoker.QTables;
using DicePoker.Utilities;

namespace DicePoker.Players;

internal class AIPlayer(QTable qTable, bool useBestAction = false) : BasePlayer
{
    private byte[] _previousState = [];
    private byte[] _reroll = [];

    public override byte[] Reroll()
    {
        _previousState = Dices.Copy();
        _reroll = useBestAction ? qTable.GetBestAction(_previousState) : qTable.GetAction(_previousState);

        _diceSet.Reroll(_reroll);

        return _reroll;
    }

    public void Learn()
    {
        qTable.UpdateQValue(_previousState, _reroll, EvaluateSet());
    }
}
