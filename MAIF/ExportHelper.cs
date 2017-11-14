using iTextSharp.text;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.html;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MAIF
{
    class ExportHelper
    {
        public static String Generate()
        {
            string templateFile = "template.docx";

            string newName = System.IO.Path.GetRandomFileName() + ".docx";
            newName = templateFile.Replace("template.docx", newName);

            File.Copy(templateFile, newName);
            //основной док-т
            DocumentFormat.OpenXml.Packaging.WordprocessingDocument docx = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(newName, true);

            string docText = null;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(docx.MainDocumentPart.GetStream()))
            {
                docText = sr.ReadToEnd();
                sr.Close();
            }

            //табличный док-т
            DocumentFormat.OpenXml.Packaging.WordprocessingDocument docx_table = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open("template_table.docx", true);

            string docText_table = null;
            string docText_table_all = null;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(docx_table.MainDocumentPart.GetStream()))
            {
                docText_table = sr.ReadToEnd();
                sr.Close();
            }

            docText_table = docText_table.Substring(docText_table.IndexOf("<w:tbl>"));
            docText_table = docText_table.Substring(0, docText_table.IndexOf("</w:tbl>") + 8);

            for (int j = 1; j < 3; j++)
            {
                for (int i = 1; i < 3; i++)
                {
                    docText_table = docText_table.Replace("%%bla-bla" + i + "%%", "пыщпыщ" + i);
                }
                //-------------------------------------------------------------------------------------------------------------------------------------------------------------

                docText_table_all += @"<w:p>
                                        <w:pPr>
                                        <w:rPr>
                                        <w:lang w:val='en-US' /> 
                                        </w:rPr>
                                        </w:pPr>
                                        </w:p>"
                                        + docText_table;
            }
            docText = docText.Replace("##table##", docText_table_all);


            using (StreamWriter sw = new StreamWriter(docx.MainDocumentPart.GetStream(FileMode.Create)))
            {
                sw.Write(docText);
                sw.Close();

            }
            docx.Close();
            
            return newName;

        }

        public static string FillTemplateHtml(Dictionary<string, string> values)
        {
            string templateFileHtml = "template.html";
            string newName = System.IO.Path.GetRandomFileName() + ".htm";
            newName = templateFileHtml.Replace("template.html", newName);

            File.Copy(templateFileHtml, newName);

            if (File.Exists(newName))
            {
                StreamReader sr = new StreamReader(newName);
                string content = sr.ReadToEnd();
                sr.Close();
                StreamWriter sw = new StreamWriter(newName);

                foreach (var v in values)
                {
                    content = content.Replace("%%" + v.Key + "%%", v.Value);
                }
                sw.Write(content);
                sw.Close();
                return newName;
            }
            else
            {
                return String.Empty;
            }

        }

        public static string FillTemplateDocx(Dictionary<string, string> values)
        {
            string templateFileHtml = "template1.docx";
            string newName = System.IO.Path.GetRandomFileName() + ".docx";
            newName = templateFileHtml.Replace("template1.docx", newName);

            File.Copy(templateFileHtml, newName);

            if (File.Exists(newName))
            {
                //основной док-т
                DocumentFormat.OpenXml.Packaging.WordprocessingDocument docx = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(newName, true);

                string content = null;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(docx.MainDocumentPart.GetStream()))
                {
                    content = sr.ReadToEnd();
                    sr.Close();
                }

                foreach (var v in values)
                {
                    content = content.Replace("%%" + v.Key + "%%", v.Value);
                }
                using (StreamWriter sw = new StreamWriter(docx.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(content);
                    sw.Close();
                }
                docx.Close();

                return newName;
            }
            else
            {
                return String.Empty;
            }
        }

        public static int PdfSharpConvert(String filename)
        {
            if (File.Exists(filename))
            {
                StreamReader sr = new StreamReader(filename);
                string html = sr.ReadToEnd();
                sr.Close();

                Byte[] res = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                    pdf.Save(ms);
                    res = ms.ToArray();

                    File.WriteAllBytes(filename.Replace(".htm", ".pdf"), res);
                    return 0;
                }
            }
            return -1;
        }

        public static int PdfSharpConvert2(String filename)
        {
            if (File.Exists(filename))
            {
                StreamReader sr = new StreamReader(filename);
                string htmlAll = sr.ReadToEnd();
                sr.Close();

                Byte[] res = null;
                // convert html to pdf  
                try
                {
                    // create a stream that we can write to, in this case a MemoryStream  
                    using (var stream = new MemoryStream())
                    {
                        // create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF  
                        using (var document = new iTextSharp.text.Document())
                        {
                            // create a writer that's bound to our PDF abstraction and our stream  
                            using (var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, stream))
                            {
                                // open the document for writing  
                                document.Open();

                                // read html data to StringReader  
                                using (var html = new StringReader(htmlAll))
                                {
                                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, html);
                                }

                                // close document  
                                document.Close();
                            }
                        }


                        res = stream.ToArray();

                        File.WriteAllBytes(filename.Replace(".htm", ".pdf"), res);
                        return 0;

                    }
                }
                catch (Exception ex)
                {
                    return -1;
                }

            }
            return -1;

        }

        public static void ConvertHtmlToPdf(string filename, string css)
        {
            if (File.Exists(filename))
            {
                StreamReader sr = new StreamReader(filename);
                string htmlAll = sr.ReadToEnd();
                sr.Close();

                using (var stream = new FileStream(filename.Replace(".htm",".pdf"), FileMode.Create))
                {
                    using (var document = new iTextSharp.text.Document())
                    {
                        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, stream);
                        document.Open();

                        // instantiate custom tag processor and add to `HtmlPipelineContext`.
                        var tagProcessorFactory = iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory();
                        //tagProcessorFactory.AddProcessor(
                        //    new TableDataProcessor(),
                        //    new string[] { iTextSharp.tool.xml.html.HTML.Tag.TD }
                        //);
                        var htmlPipelineContext = new iTextSharp.tool.xml.pipeline.html.HtmlPipelineContext(null);
                        htmlPipelineContext.SetTagFactory(tagProcessorFactory);

                        var pdfWriterPipeline = new iTextSharp.tool.xml.pipeline.end.PdfWriterPipeline(document, writer);
                        var htmlPipeline = new HtmlPipeline(htmlPipelineContext, pdfWriterPipeline);

                        // get an ICssResolver and add the custom CSS
                        var cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                        cssResolver.AddCss(css, "utf-8", true);
                        var cssResolverPipeline = new CssResolverPipeline(
                            cssResolver, htmlPipeline
                        );

                        var worker = new XMLWorker(cssResolverPipeline, true);
                        var parser = new XMLParser(worker);
                        using (var stringReader = new StringReader(htmlAll))
                        {
                            parser.Parse(stringReader);
                        }
                    }
                }
            }
        }
        public static void ConvertWord2PDF(string inputFile, string outputPath)
        {
            try
            {
                if (outputPath.Equals("") || !File.Exists(inputFile))
                {
                    throw new Exception("Either file does not exist or invalid output path");
                }

                // app to open the document belower
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document wordDocument = new Microsoft.Office.Interop.Word.Document();

                // input variables
                object objInputFile = inputFile;
                object missParam = Type.Missing;

                wordDocument = wordApp.Documents.Open(ref objInputFile, ref missParam, ref missParam, ref missParam,
                    ref missParam, ref missParam, ref missParam, ref missParam, ref missParam, ref missParam,
                    ref missParam, ref missParam, ref missParam, ref missParam, ref missParam, ref missParam);

                if (wordDocument != null)
                {
                    // make the convertion
                    wordDocument.ExportAsFixedFormat(outputPath, WdExportFormat.wdExportFormatPDF, false,
                        WdExportOptimizeFor.wdExportOptimizeForOnScreen, WdExportRange.wdExportAllDocument,
                        0, 0, WdExportItem.wdExportDocumentContent, true, true,
                        WdExportCreateBookmarks.wdExportCreateWordBookmarks, true, true, false, ref missParam);
                }

                // close document and quit application
                wordDocument.Close();
                wordApp.Quit();


            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}