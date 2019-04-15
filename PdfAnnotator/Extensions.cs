using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static (float Llx, float Lly, float Urx, float Ury) GetPdfCoordsIText5(this Pdf.IWord target)
        {
            var pageWidth = target.Parent.Width;
            var pageHeight = target.Parent.Height;
            var llx = target.XMin;
            var lly = pageHeight - target.YMax;
            var urx  = target.XMax;
            var ury = pageHeight - target.YMin;
            return (llx, lly, urx, ury);
        }

        public static (float Llx, float Lly, float width, float height) GetPdfCoords(this Pdf.IWord target)
        {
            var pageWidth = target.Parent.Width;
            var pageHeight = target.Parent.Height;
            var llx = target.XMin;
            var lly = pageHeight - target.YMax;
            var width = target.XMax - target.XMin;
            var height = target.YMax - target.YMin;
            return (llx, lly, width, height);
        }
    }
}
