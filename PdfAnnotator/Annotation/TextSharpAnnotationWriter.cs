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
                        var txRect = GetAnnotationRect(buttonRect, font, ann, trg);

                        var textFieldName = Guid.NewGuid().ToString("n");
                        var textField = PdfFormField.CreateText(doc, txRect, textFieldName);
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

                        var underline = PdfTextMarkupAnnotation.CreateUnderline(new Rectangle(buttonRect.GetX(), buttonRect.GetY() - 2, buttonRect.GetWidth(), 3),
                            new float[]
                            {
                                buttonRect.GetX() + buttonRect.GetWidth(), buttonRect.GetY() + buttonRect.GetHeight(),
                                buttonRect.GetX(), buttonRect.GetY() + buttonRect.GetHeight(),
                                buttonRect.GetX() + buttonRect.GetWidth(), buttonRect.GetY() - 2,
                                buttonRect.GetX(), buttonRect.GetY() - 2,
                            });
                        underline.SetColor(ColorConstants.YELLOW);
                        page.AddAnnotation(underline);
                    }
                }
            }
            return Task.CompletedTask;
        }

        private Rectangle GetAnnotationRect(Rectangle buttonRect, PdfFont font, IAnnotation annotation, Pdf.IWord target)
        {
            var rectU = GetUpperOrLowerAnnotationRect(buttonRect, font, annotation, target, true);
            var rect = rectU;
            if (rectU.GetY() + rectU.GetHeight() > target.Parent.Height)
            {
                var rectL = GetUpperOrLowerAnnotationRect(buttonRect, font, annotation, target, false);
                if (rectL.GetY() + rectL.GetHeight() < target.Parent.Height && rectL.GetY() > 0) rect = rectL;
            }

            if (rect.GetX() < 5 && rect.GetX() + 5 < target.Parent.Width)
            {
                // text is left outside of page but smaller than page plus some margin so move it inside
                rect.SetX(5);
            }
            else
            {
                var rectMaxX = rect.GetX() + rect.GetWidth();
                var delta = rectMaxX + 5 - target.Parent.Width;
                if (delta > 0 && rect.GetWidth() < target.Parent.Width)
                {
                    // text is right outside of page but smaller than page so move it inside
                    rect.SetX(rect.GetX() - delta);
                }
            }

            return rect;
        }

        private Rectangle GetUpperOrLowerAnnotationRect(Rectangle buttonRect, PdfFont font, IAnnotation annotation, Pdf.IWord target, bool upper = true)
        {
            var contentBounds = GetContentBounds(annotation.Content, target.Parent.Width / 2, font);

            var targetWidthHalf = buttonRect.GetWidth() / 2;
            var helpWidthHalf = contentBounds.width / 2;
            var annotHeight = contentBounds.height + 6;
            float annotY;
            if (upper)
            {
                annotY = buttonRect.GetY() + buttonRect.GetHeight() + 5;
            } else
            {
                annotY = buttonRect.GetY() - annotHeight - 10;
            }
            var helpRect = new Rectangle(buttonRect.GetX() + targetWidthHalf - helpWidthHalf - 5, annotY, contentBounds.width + 10, annotHeight);
            return helpRect;
        }

        private (float height, float width) GetContentBounds(string content, float maxWidth, PdfFont font)
        {
            if (string.IsNullOrEmpty(content)) return (0, 0);
            float height = 0;
            float width = 1;
            foreach (var contentLine in content.Split('\n'))
            {
                var lineWidth = Math.Max(font.GetWidth(contentLine, FontSize), 1);
                var visibleContentWidth = Math.Min(maxWidth, lineWidth);
                var lineHeight = (FontSize + 2) * (float)Math.Ceiling(lineWidth / visibleContentWidth);
                height += lineHeight;
                width = Math.Max(width, visibleContentWidth);
            }
            
            return (height, width);
        }
    }
}
