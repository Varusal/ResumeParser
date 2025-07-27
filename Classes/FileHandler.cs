using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace ResumeParser.Classes
{
    internal class FileHandler
    {
        private string pdfContent;

        public FileHandler()
        {
            pdfContent = string.Empty;
        }

        public string GetResumeFile(string path)
        {
            Console.Clear();

            PdfDocument pdfDoc = PdfDocument.Open(path);
            int pdfPageCount = pdfDoc.NumberOfPages;

            for (int i = 1; i <= pdfDoc.NumberOfPages; i++)
            {
                Page pdfPage = pdfDoc.GetPage(i);
                pdfContent += pdfPage.Text;
            }

            return pdfContent;
        }
    }
}
