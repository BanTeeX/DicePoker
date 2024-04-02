namespace DicePoker;

internal class DiceSet
{
    private readonly byte[] _dices = new byte[5];

    public byte[] Dices => _dices;

    public void Roll()
    {
        for (int i = 0; i < _dices.Length; i++)
        {
            _dices[i] = (byte)(Random.Shared.Next(6) + 1);
        }

        Array.Sort(_dices);
    }

    public void Reroll(params byte[] dicesToReroll)
    {
        for (int i = 0; i < dicesToReroll.Length; i++)
        {
            var index = Array.IndexOf(_dices, dicesToReroll[i]);
            _dices[index] = (byte)(Random.Shared.Next(6) + 1);
        }

        Array.Sort(_dices);
    }

    public double EvaluateSet()
    {
        byte[] set = new byte[6];

        for (int i = 0; i < _dices.Length; i++)
        {
            set[_dices[i] - 1]++;
        }

        //5 of Kind
        if (set.Any(x => x == 5))
        {
            var index = Array.IndexOf(set, set.First(x => x == 5));

            return (246 + index) / 251.0;
        }

        //4 of Kind
        if (set.Any(x => x == 4))
        {
            var index = Array.IndexOf(set, set.First(x => x == 4));

            var tmpSet = set.Where(x => x != 4).ToArray();
            var index2 = Array.IndexOf(tmpSet, tmpSet.First(x => x == 1));

            return (216 + index * 5 + index2) / 251.0;
        }

        //Full
        if (set.Any(x => x == 3) && set.Any(x => x == 2))
        {
            var index = Array.IndexOf(set, set.First(x => x == 3));

            var tmpSet = set.Where(x => x != 3).ToArray();
            var index2 = Array.IndexOf(tmpSet, tmpSet.First(x => x == 2));

            return (186 + index * 5 + index2) / 251.0;
        }

        //High Straight
        if (set[5] == 1 && set[4] == 1 && set[3] == 1 && set[2] == 1 && set[1] == 1)
        {
            return 185 / 251.0;
        }

        //Small Straight
        if (set[4] == 1 && set[3] == 1 && set[2] == 1 && set[1] == 1 && set[0] == 1)
        {
            return 184 / 251.0;
        }

        //3 of Kind
        if (set.Any(x => x == 3) && set.Any(x => x == 1))
        {
            var index = Array.IndexOf(set, set.First(x => x == 3));

            var tmpSet = set.Where(x => x != 3).ToArray();
            var index2 = Array.IndexOf(tmpSet, tmpSet.Where(x => x == 1));
            var index3 = Array.LastIndexOf(tmpSet, tmpSet.Where(x => x == 1));

            var modifier = index2 + index3 - 1;
            modifier += index3 switch
            {
                3 => 1,
                4 => 3,
                _ => 0
            };

            return (124 + index * 10 + modifier) / 251.0;
        }

        //2 Pairs
        if (set.Where(x => x == 2).Count() == 2)
        {
            var index = Array.LastIndexOf(set, set.First(x => x == 2));
            var index2 = Array.IndexOf(set, set.First(x => x == 2));

            var modifier = index + index2 - 1;
            modifier += index switch
            {
                3 => 1,
                4 => 3,
                5 => 6,
                _ => 0
            };

            var tmpSet = set.Where(x => x != 2).ToArray();
            var index3 = Array.IndexOf(tmpSet, tmpSet.First(x => x == 1));

            return (64 + modifier * 4 + index3) / 251.0;
        }

        //Pair
        if (set.Any(x => x == 2))
        {
            var index = Array.IndexOf(set, set.First(x => x == 2));

            var tmpSet = set.Where(x => x != 2).ToArray();
            var index2 = Array.IndexOf(tmpSet, tmpSet.First(x => x == 0));
            var index3 = Array.LastIndexOf(tmpSet, tmpSet.First(x => x == 0));

            var modifier = 10 - index2 - index3;
            modifier -= index3 switch
            {
                3 => 1,
                4 => 3,
                _ => 0
            };

            return (4 + index * 10 + modifier) / 251.0;
        }

        //High
        {
            var index = Array.IndexOf(set, set.First(x => x == 0));
            return (index - 1) / 251.0;
        }
    }
}
