using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResumeParser.Classes;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace ResumeParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Things I can improve:
            // Add context -> save all inputs and outputs to prompt
            // Add support for other file types txt doc docx odf xls xlsx maybe even csv

            HttpClient client = new HttpClient();

            string pdfContent = string.Empty;
            string userPrompt = string.Empty;

            FileHandler fHandler = new FileHandler();
            AIHandler aiHandler = new AIHandler();
            HTTPHandler httpHandler = new HTTPHandler();

            aiHandler.Model = "gpt-4.1-nano";
            aiHandler.Uri = "chat/completions";
            aiHandler.Prompt = "Summarize the following job applicaton: ";
            aiHandler.BaseURI = new Uri("https://api.openai.com/v1/");

            Console.Write("Enter api key: ");
            aiHandler.ApiKey = Console.ReadLine();

            client = httpHandler.CreateClient(aiHandler.BaseURI, new AuthenticationHeaderValue("Bearer", aiHandler.ApiKey));

            Console.Clear();

            Console.Write("Enter path to job application (pdf): ");
            pdfContent = fHandler.GetResumeFile(Console.ReadLine());

            Console.Clear();

            do
            {
                Console.WriteLine(httpHandler.PostPrompt(aiHandler, client, userPrompt, pdfContent));

                Console.Write("> ");
                userPrompt = Console.ReadLine();
                Console.WriteLine();

            } while (userPrompt != string.Empty);

            Console.Write("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
