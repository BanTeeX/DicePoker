using DicePoker.Utilities;

namespace DicePoker.QTables;

internal static class QTableGenerator
{
    private static readonly Random _random = new();

    public static Dictionary<byte[], Dictionary<byte[], double>> GenerateQTable()
    {
        var qTable = new Dictionary<byte[], Dictionary<byte[], double>>(ArrayComparer<byte>.Instance);

        var states = GenerateStates();
        foreach (var state in states)
        {
            var actions = GenerateActionsForState(state);

            var actionsSet = new Dictionary<byte[], double>(ArrayComparer<byte>.Instance);
            foreach (var action in actions)
            {
                actionsSet.Add(action, _random.NextDouble());
            }

            qTable.Add(state, actionsSet);
        }

        return qTable;
    }

    private static HashSet<byte[]> GenerateStates()
    {
        var sets = new HashSet<byte[]>(ArrayComparer<byte>.Instance);

        byte[] set = [1, 1, 1, 1, 1];

        while (true)
        {
            sets.Add(set.Copy());

            int index = 4;

            while (index >= 0 && set[index] == 6)
            {
                index--;
            }

            if (index < 0)
            {
                break;
            }

            set[index]++;

            for (int i = index + 1; i < 5; i++)
            {
                set[i] = set[index];
            }
        }

        return sets;
    }

    private static HashSet<byte[]> GenerateActionsForState(byte[] state)
    {
        var actions = new HashSet<byte[]>(ArrayComparer<byte>.Instance)
        {
            ([])
        };

        for (int i = 1; i < state.Length; i++)
        {
            var indexSets = GetIndexSets(state.Length, i);
            foreach (var indexSet in indexSets)
            {
                var action = indexSet.Select(index => state[index]).ToArray();
                actions.Add(action);
            }
        }

        actions.Add(state.Copy());

        return actions;
    }

    private static List<int[]> GetIndexSets(int n, int k)
    {
        List<int[]> sets = [];

        var set = new int[k];

        for (int i = 0; i < k; i++)
        {
            set[i] = i;
        }

        while (true)
        {
            sets.Add(set.Copy());

            int index = k - 1;

            while (index >= 0 && set[index] == n + index - k)
            {
                index--;
            }

            if (index < 0)
            {
                break;
            }

            set[index]++;

            for (int i = index + 1; i < k; i++)
            {
                set[i] = set[i - 1] + 1;
            }
        }

        return sets;
    }
}
