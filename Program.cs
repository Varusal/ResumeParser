using ResumeParser.Classes;
using System.Net.Http.Headers;

namespace ResumeParser
{
    internal class Program
    {
        // Todo: Add menu options
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();

            string fileContent = string.Empty;
            string userPrompt = string.Empty;
            bool filePathCorrect = false;
            bool fileExists = false;

            FileHandler fHandler = new FileHandler();
            AIHandler aiHandler = new AIHandler();
            HTTPHandler httpHandler = new HTTPHandler();

            aiHandler.Model = "gpt-4.1-nano";
            aiHandler.Uri = "chat/completions";
            aiHandler.Prompt = "Summarize the following job applicaton: ";
            aiHandler.BaseURI = new Uri("https://api.openai.com/v1/");

            do
            {
                Console.Write("Enter api key: ");
                aiHandler.ApiKey = Console.ReadLine();
                if (string.IsNullOrEmpty(aiHandler.ApiKey))
                {
                    Console.WriteLine("API key cannot be empty. Please try again.");
                    Console.WriteLine();
                }
            } while (string.IsNullOrEmpty(aiHandler.ApiKey));

            client = httpHandler.CreateClient(aiHandler.BaseURI, new AuthenticationHeaderValue("Bearer", aiHandler.ApiKey));

            Console.Clear();

            do
            {
                Console.Write("Enter path to job application: ");
                string filePath = Console.ReadLine();

                if (string.IsNullOrEmpty(filePath))
                {
                    Console.WriteLine("File path cannot be empty. Please try again.");
                    Console.WriteLine();
                    filePathCorrect = false;
                }
                else
                {
                    filePathCorrect = true;

                    if (File.Exists(filePath))
                    {
                        fileExists = true;
                        fileContent = fHandler.GetResumeFile(filePath);
                    }
                    else
                    {
                        Console.WriteLine("File does not exist. Please try again.");
                        Console.WriteLine();
                        fileExists = false;
                    }
                }

            } while (!filePathCorrect || !fileExists);

            Console.Clear();

            do
            {
                Console.WriteLine(httpHandler.PostPrompt(aiHandler, client, userPrompt, fileContent));

                if (!httpHandler.RequestError)
                {
                    Console.Write("> ");
                    userPrompt = Console.ReadLine();
                    Console.WriteLine();
                }

            } while (!string.IsNullOrEmpty(userPrompt));

            Console.Write("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
