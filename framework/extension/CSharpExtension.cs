using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace framework.extension;

public static class CollectionsExtension {
    private static readonly Random Rnd = new();

    public static bool IsNullOrEmpty<T>(this ICollection<T> collection) {
        return collection == null || collection.Count == 0;
    }
    
    public static T PopFront<T>(this IList<T> collection) {
        if (collection.IsNullOrEmpty()) {
            return default;
        }

        T firstItem = collection[0];
        collection.RemoveAt(0);
        return firstItem;
    }

    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = Rnd.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}

public static class StringExtension {
    public static bool IsNullOrEmpty(this string str) {
        return string.IsNullOrEmpty(str);
    }
}