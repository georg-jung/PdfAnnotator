using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfAnnotator.Annotation
{
    internal class TextSharpAnnotationWriter : IAnnotationWriter
    {
        public int FontSize { get; set; } = 10;

        public Task WriteAnnotatedPdfAsync(string pdfDocumentPath, IEnumerable<IAnnotation> annotations, string filePath)
        {
            using (var reader = new PdfReader(pdfDocumentPath))
            using (var outFile = File.Open(filePath, FileMode.Create))
            using (var stamper = new PdfStamper(reader, outFile))
            {
                foreach (var ann in annotations)
                {
                    var targets = ann.SelectedTargets;
                    if (targets == null || targets.Count < 1) targets = ann.Subject.Appearances ?? throw new ArgumentException($"The subject word {ann.Subject.Text} for a given annotation has no appearances.");
                    foreach (var trg in targets)
                    {
                        var font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                        var coord = trg.GetPdfCoords();
                        var buttonRect = new iTextSharp.text.Rectangle(coord.Llx, coord.Lly, coord.Urx, coord.Ury);
                        var contentWidth = font.GetWidthPoint(ann.Content, FontSize);
                        var helpWith = Math.Min(trg.Parent.Width / 2, contentWidth);
                        var helpHeight = 13 * ((float)Math.Ceiling(contentWidth / helpWith) + ann.Content.Count(x => x == '\n') );
                        var targetWidthHalf = (coord.Urx - coord.Llx) / 2;
                        var helpWidthHalf = helpWith / 2;
                        var helpRect = new iTextSharp.text.Rectangle(coord.Llx + targetWidthHalf - helpWidthHalf - 5, coord.Ury + 5, coord.Urx - targetWidthHalf + helpWidthHalf + 5, coord.Ury + 5 + helpHeight + 6);

                        var textField = new TextField(stamper.Writer, helpRect, Guid.NewGuid().ToString("n"))
                        {
                            Text = ann.Content,
                            FontSize = FontSize,
                            Font = font,
                            TextColor = BaseColor.DARK_GRAY,
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            Options = BaseField.MULTILINE | BaseField.READ_ONLY,
                            Visibility = BaseField.HIDDEN,
                            BorderColor = BaseColor.LIGHT_GRAY
                        };
                        textField.SetExtraMargin(2f, 2f);
                        textField.Alignment = Element.ALIGN_TOP | Element.ALIGN_CENTER;
                        var textWidget = textField.GetTextField();
                        textWidget.SetFieldFlags(4097);
                        stamper.AddAnnotation(textWidget, trg.Parent.Index + 1);

                        var button = new PushbuttonField(stamper.Writer, buttonRect, Guid.NewGuid().ToString("n"));
                        var buttonWidget = button.Field;
                        var enter = PdfAction.CreateHide(textWidget, false);
                        buttonWidget.SetAdditionalActions(PdfName.E, enter);
                        var exit = PdfAction.CreateHide(textWidget, true);
                        buttonWidget.SetAdditionalActions(PdfName.X, exit);
                        stamper.AddAnnotation(buttonWidget, trg.Parent.Index + 1);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
