using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Colorspace;
using iText.StyledXmlParser.Jsoup.Nodes;
using Rectangle = iText.Kernel.Geom.Rectangle;

namespace PdfAnnotator.Annotation
{
    internal class TextSharpAnnotationWriter : IAnnotationWriter
    {
        public int FontSize { get; set; } = 10;

        public Task WriteAnnotatedPdfAsync(string pdfDocumentPath, IEnumerable<IAnnotation> annotations, string filePath)
        {
            using (var reader = new PdfReader(pdfDocumentPath))
            using (var outFile = File.Open(filePath, FileMode.Create))
            using (var writer = new PdfWriter(outFile))
            using (var doc = new PdfDocument(reader, writer))
            {
                var acroForm = PdfAcroForm.GetAcroForm(doc, true);
                foreach (var ann in annotations)
                {
                    var targets = ann.SelectedTargets;
                    if (targets == null || targets.Count < 1) targets = ann.Subject.Appearances ?? throw new ArgumentException($"The subject word {ann.Subject.Text} for a given annotation has no appearances.");
                    foreach (var trg in targets)
                    {
                        var page = doc.GetPage(trg.Parent.Index + 1);

                        var font = PdfFontFactory.CreateRegisteredFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);
                        var coord = trg.GetPdfCoords();
                        var buttonRect = new Rectangle(coord.Llx, coord.Lly, coord.width, coord.height);
                        var contentWidth = font.GetWidth(ann.Content, FontSize);
                        var helpWidth = Math.Min(trg.Parent.Width / 2, contentWidth);
                        var helpHeight = 12 * ((float)Math.Ceiling(contentWidth / helpWidth) + ann.Content.Count(x => x == '\n'));
                        var targetWidthHalf = coord.width / 2;
                        var helpWidthHalf = helpWidth / 2;
                        var helpRect = new Rectangle(coord.Llx + targetWidthHalf - helpWidthHalf - 5, coord.Lly + coord.height + 5, helpWidth + 10, helpHeight + 6);

                        var textFieldName = Guid.NewGuid().ToString("n");
                        var textField = PdfFormField.CreateText(doc, helpRect, textFieldName);
                        textField.SetValue(ann.Content, font, FontSize);
                        textField.SetColor(ColorConstants.DARK_GRAY);
                        textField.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                        textField.SetReadOnly(true);
                        textField.SetMultiline(true);
                        textField.SetVisibility(PdfFormField.HIDDEN);
                        textField.SetBorderColor(ColorConstants.LIGHT_GRAY);
                        textField.SetFieldFlags(4097);

                        acroForm.AddField(textField, page);
                        
                        var enter = PdfAction.CreateHide(textFieldName, false);
                        var exit = PdfAction.CreateHide(textFieldName, true);
                        
                        var btn = PdfFormField.CreatePushButton(doc, buttonRect, Guid.NewGuid().ToString("n"),
                            string.Empty);
                        btn.SetBackgroundColor(null);
                        btn.SetBorderWidth(0);
                        btn.SetAdditionalAction(PdfName.E, enter);
                        btn.SetAdditionalAction(PdfName.X, exit);
                        acroForm.AddField(btn, page);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
