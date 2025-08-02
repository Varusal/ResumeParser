using ResumeParser.Classes;
using System.Data;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Fonts.TrueType.Names;

namespace ResumeParser
{
    internal class Program
    {
        // Todo: Add menu options
        static void Main(string[] args)
        {
            List<MenuOptions> menu = new List<MenuOptions>();
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

            CreateMenu(menu);
            #region Menu testing
            // To do: loop indefinitely until user exits
            bool renderMenu = true;
            do
            {
                for (int i = 0; i < menu.Count; i++)
                {
                    Console.WriteLine(i + ". " + menu[i].Message);
                }

                Console.WriteLine();
                Console.Write("Select menu option: ");
                int menuChoice = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                menu[menuChoice].Method.Invoke();

            } while (renderMenu);
            #endregion


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

        private static void CreateMenu(List<MenuOptions> menu)
        {
            MenuOptions menuOption = new MenuOptions
            {
                Name = "APIKey",
                Message = "Change API Key.",
                Method = () => { }
            };

            menu.Add(menuOption);
        }

        private static void ChangeAPIKey(AIHandler aiHandler)
        {
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
        }
    }
}
