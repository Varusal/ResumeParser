using NPOI.XWPF.Extractor;
using NPOI.XWPF.UserModel;
using System.IO.Compression;
using System.Xml;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace ResumeParser.Classes
{
    internal class FileHandler
    {
        private string fileContent;
        private string filePath;

        public FileHandler()
        {
            fileContent = string.Empty;
            filePath = string.Empty;
        }

        public string GetResumeFile(string path)
        {
            Console.Clear();
            filePath = path;
            FileInfo file = new FileInfo(filePath);

            switch (file.Extension)
            {
                case ".pdf":
                    ReadPDF();
                    break;
                case ".txt":
                    ReadTXT();
                    break;
                case ".doc":
                    ReadDOC();
                    break;
                case ".docx":
                    ReadDOC();
                    break;
                case ".odf":
                    ReadOpenDOC();
                    break;
                case ".ott":
                    ReadOpenDOC();
                    break;
                case ".odt":
                    ReadOpenDOC();
                    break;
                default:
                    break;
            }

            return fileContent;
        }

        private void ReadPDF()
        {
            PdfDocument pdfDoc = PdfDocument.Open(filePath);
            int pdfPageCount = pdfDoc.NumberOfPages;

            for (int i = 1; i <= pdfDoc.NumberOfPages; i++)
            {
                Page pdfPage = pdfDoc.GetPage(i);
                fileContent += pdfPage.Text;
            }
            pdfDoc.Dispose();
        }

        private void ReadTXT()
        {
            foreach (string word in File.ReadAllLines(filePath))
            {
                fileContent += word;
            }
        }

        private void ReadDOC()
        {
            XWPFDocument doc = new XWPFDocument(File.OpenRead(filePath));
            XWPFWordExtractor docExtractor = new XWPFWordExtractor(doc);
            fileContent = docExtractor.Text;
        }

        private void ReadOpenDOC()
        {
            ZipArchive zip = ZipFile.OpenRead(filePath);
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (entry.FullName == "content.xml")
                {
                    StreamReader reader = new StreamReader(entry.Open());
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(reader);
                    fileContent += xmlDoc.InnerText;
                    reader.Close();
                    break;
                }
            }
        }
    }
}
