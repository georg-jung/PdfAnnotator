using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;

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

        public static (float Llx, float Lly, float Urx, float Ury) GetPdfCoords(this Pdf.IWord target)
        {
            var w = target.Parent.Width;
            var h = target.Parent.Height;
            var llx = target.XMin;
            var lly = h - target.YMax;
            var urx  = target.XMax;
            var ury = h - target.YMin;
            return (llx, lly, urx, ury);
        }
    }
}
