using System.Collections;

namespace Frank.XsdCodeGeneration;

public static class ObjectExtensions
{
    public static void Dump(this object? value)
    {
        Console.WriteLine(value);
    }

    public static IEnumerable<TResult> OfType<TResult>(this IEnumerable sources)
    {
        foreach (var source in sources)
            if (source is TResult result)
                yield return result;
    }

    public static TResult? OfType<TResult>(this object? source)
    {
        if (source is TResult result)
            return result;
        return default;
    }
}