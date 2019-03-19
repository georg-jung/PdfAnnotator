using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using PdfAnnotator.Annotation;
using PdfAnnotator.Pdf;

namespace PdfAnnotator.Persistence.Model
{
    internal class WordAnnotation
    {
        public Guid Id { get; set; }
        [BsonRef]
        public PdfFile Document { get; set; }
        public string Word { get; set; }
        public string Content { get; set; }
    }
}
