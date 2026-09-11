using System;
using System.Collections.Generic;

namespace AutoMouseKeyboard.Utilities
{
    public static class KeyHelper
{
    public static IEnumerable<Tuple<string, string>> EnumerateKeys(Func<int, string> selector, int start, int count)
    {
        for (var i = 0; i < count; i++)
        {
            var token = selector(start + i);
            yield return Tuple.Create(token, token);
        }
    }

    public static IEnumerable<Tuple<string, string>> ToOptions(IEnumerable<string> keys)
    {
        foreach (var key in keys)
        {
            yield return Tuple.Create(key, key);
        }
    }
    }
}

