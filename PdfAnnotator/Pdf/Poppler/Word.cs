namespace PdfAnnotator.Pdf.Poppler
{
    internal class Word : IWord
    {
        public string Text { get; set; }
        public float XMin { get; set; }
        public float YMin { get; set; }
        public float XMax { get; set; }
        public float YMax { get; set; }
        public Page Parent { get; set; }

        IPage IWord.Parent => Parent;
    }
}
