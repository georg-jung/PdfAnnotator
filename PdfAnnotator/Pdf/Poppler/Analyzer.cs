using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using PdfAnnotator.Utils;

namespace PdfAnnotator.Pdf.Poppler
{
    internal class Analyzer : IAnalyzer
    {
        private static readonly string PdfToTextArgs = "-bbox";
        private static readonly int PdfToTextTimeout = 60000;

        private static string GetPdfToTextExePath()
        {
            // see https://stackoverflow.com/questions/837488/how-can-i-get-the-applications-path-in-a-net-console-application
            var dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("Could not determine assembly's path.");
            return System.IO.Path.Combine(dir, "poppler", "bin", "pdftotext.exe");
        }

        public async Task<IAnalysis> AnalyzeAsync(string pdfPath, IProgress<int> pageProgress = null, CancellationToken ct = default)
        {
            var p2t = GetPdfToTextExePath();
            var output = System.IO.Path.GetTempFileName();
            var arg = $"{PdfToTextArgs} \"{pdfPath}\" \"{output}\"";
            var res = await ProcessAsyncHelper.RunProcessAsync(p2t, arg, PdfToTextTimeout).ConfigureAwait(false);
            if (res.ExitCode != 0) throw new ApplicationException($"PdfToText exited with code {res.ExitCode?.ToString() ?? "null"}. StdErr: {res.Error}");
            var analysis = await ParseXmlAsync(output, pageProgress, ct).ConfigureAwait(false);
            System.IO.File.Delete(output);
            return analysis;
        }

        private static async Task<Analysis> ParseXmlAsync(string htmlOutputPath, IProgress<int> pageProgress = null, CancellationToken ct = default)
        {
            var settings = new XmlReaderSettings { Async = true, DtdProcessing = DtdProcessing.Ignore, CheckCharacters = false };

            using (var stream = new FileStream(htmlOutputPath, FileMode.Open, FileAccess.Read))
            using (var xmlFriendlyStream = new InvalidXmlCharacterReplacingStreamReader(stream, '_'))
            using (var reader = XmlReader.Create(xmlFriendlyStream, settings))
            {
                var inDoc = false;
                var inPage = false;
                var pages = new List<Page>();
                var analysis = new Analysis(pages);
                var curWords = new List<Word>();
                var curPage = new Page(curWords, analysis, 0);
                Word curWord = null;
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    ct.ThrowIfCancellationRequested();
                    if (reader.Name == "doc")
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            if (inDoc) throw new ArgumentException("Opening doc-Element but we already are in doc."); else inDoc = true;
                        if (reader.NodeType == XmlNodeType.EndElement)
                            if (!inDoc) throw new ArgumentException("Closing doc-Element but we are not in doc."); else return analysis;
                    }
                    if (!inDoc) continue;
                    if (reader.Name == "page")
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            if (inPage) throw new ArgumentException("Opening page-Element but we already are in page.");
                            else
                            {
                                inPage = true;
                                curPage.Width = float.Parse(reader.GetAttribute("width") ?? throw new ArgumentException("page-Element has no width-attribute."), CultureInfo.InvariantCulture);
                                curPage.Height = float.Parse(reader.GetAttribute("height") ?? throw new ArgumentException("page-Element has no height-attribute."), CultureInfo.InvariantCulture);
                            }
                        if (reader.NodeType == XmlNodeType.EndElement)
                            if (!inPage) throw new ArgumentException("Closing page-Element but we are not in page.");
                            else
                            {
                                pages.Add(curPage);
                                curWords = new List<Word>();
                                curPage = new Page(curWords, analysis, pages.Count);
                                inPage = false;
                                pageProgress?.Report(pages.Count);
                            }
                    }
                    if (!inPage) continue;
                    if (reader.Name == "word")
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            if (curWord != null) throw new ArgumentException("Opening word-Element but we already are in word.");
                            else
                            {
                                curWord = new Word
                                {
                                    XMin = float.Parse(reader.GetAttribute("xMin") ?? throw new ArgumentException("word-Element has no xMin-attribute."), CultureInfo.InvariantCulture),
                                    YMin = float.Parse(reader.GetAttribute("yMin") ?? throw new ArgumentException("word-Element has no yMin-attribute."), CultureInfo.InvariantCulture),
                                    XMax = float.Parse(reader.GetAttribute("xMax") ?? throw new ArgumentException("word-Element has no xMax-attribute."), CultureInfo.InvariantCulture),
                                    YMax = float.Parse(reader.GetAttribute("yMax") ?? throw new ArgumentException("word-Element has no yMax-attribute."), CultureInfo.InvariantCulture),
                                    Parent = curPage
                                };
                            }
                        if (reader.NodeType == XmlNodeType.EndElement)
                            if (curWord == null) throw new ArgumentException("Closing doc-Element but we are not in doc.");
                            else
                            {
                                curWords.Add(curWord);
                                curWord = null;
                            }
                    }

                    if (curWord != null && reader.NodeType == XmlNodeType.Text)
                    {
                        curWord.Text = await reader.GetValueAsync().ConfigureAwait(false);
                    }
                }
                throw new ArgumentException("Given html is not valid. Maybe the closing </doc> tag is missing?");
            }
        }
    }
}
