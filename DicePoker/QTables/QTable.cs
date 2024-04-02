using DicePoker.Utilities;
using Newtonsoft.Json;

namespace DicePoker.QTables;

internal class QTable(bool generate, string path, double learningRate = 0.1, double explorationRate = 0.1)
{
    private readonly Dictionary<byte[], Dictionary<byte[], double>> _qTable = InitializeQTable(generate, path);
    private readonly object _qTableLock = new();
    private readonly Random _random = new();

    private static Dictionary<byte[], Dictionary<byte[], double>> InitializeQTable(bool generate, string path)
    {
        if (generate)
        {
            return QTableGenerator.GenerateQTable();
        }

        var json = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<Dictionary<byte[], Dictionary<byte[], double>>>(
            json,
            new JsonQTableConverter())!;
    }

    public void SaveInFile()
    {
        var json = string.Empty;

        lock (_qTableLock)
        {
            json = JsonConvert.SerializeObject(_qTable, Formatting.Indented, new JsonQTableConverter());
        }

        File.WriteAllText(path, json);
    }

    public byte[] GetAction(byte[] state)
    {
        if (_random.NextDouble() < explorationRate)
        {
            var actions = _qTable[state].Keys.ToArray();

            lock (_qTableLock)
            {
                return actions[_random.Next(actions.Length)];
            }
        }

        return GetBestAction(state);
    }

    public void UpdateQValue(byte[] state, byte[] action, double reward)
    {
        lock (_qTableLock)
        {
            var qValue = _qTable[state][action];
            _qTable[state][action] += (reward - qValue) * learningRate;
        }
    }

    public byte[] GetBestAction(byte[] state)
    {
        lock (_qTableLock)
        {
            return _qTable[state].OrderByDescending(x => x.Value).First().Key;
        }
    }
}
