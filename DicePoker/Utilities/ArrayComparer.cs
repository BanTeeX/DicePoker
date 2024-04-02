using System.Diagnostics.CodeAnalysis;

namespace DicePoker.Utilities;

internal class ArrayComparer<T> : IEqualityComparer<T[]>
{
    public static ArrayComparer<T> Instance = new();

    public bool Equals(T[]? x, T[]? y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null && y is not null)
        {
            return false;
        }

        if (x is not null && y is null)
        {
            return false;
        }

        if (x!.Length != y!.Length)
        {
            return false;
        }

        for (int i = 0; i < x.Length; i++)
        {
            if (!x[i]!.Equals(y[i]))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode([DisallowNull] T[] obj)
    {
        int hash = 0;

        foreach (var item in obj)
        {
            hash += item is null ? 0 : item.GetHashCode();
        }

        return hash;
    }
}
