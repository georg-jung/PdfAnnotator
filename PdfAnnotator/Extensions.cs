using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfAnnotator
{
    internal static class Extensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
            TValue defaultValue = default)
        {
            if (dict.TryGetValue(key, out var val)) return val;
            return defaultValue;
        }

        public static TValue AddAndReturn<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict.Add(key, value);
            return value;
        }
    }
}
