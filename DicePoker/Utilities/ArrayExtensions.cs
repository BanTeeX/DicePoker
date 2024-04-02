namespace DicePoker.Utilities;

internal static class ArrayExtensions
{
    public static T[] Copy<T>(this T[] tab)
    {
        var copy = new T[tab.Length];
        tab.CopyTo(copy, 0);
        return copy;
    }
}
