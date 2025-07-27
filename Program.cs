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
            HttpClient client = new HttpClient();

            string apiKey = string.Empty;
            string model = "gpt-4.1-nano";
            string uri = "chat/completions";
            string prompt = "Summarize the following job applicaton: ";
            string pdfContent = string.Empty;
            string userPrompt = string.Empty;
            string chatComplID = string.Empty;

            FileHandler fhandler = new FileHandler();

            Console.Write("Enter api key: ");
            apiKey = Console.ReadLine();

            client.BaseAddress = new Uri("https://api.openai.com/v1/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            Console.Clear();

            Console.Write("Enter path to job application (pdf): ");
            prompt += fhandler.GetResumeFile(Console.ReadLine());

            Console.Clear();

            

            do
            {
                if (userPrompt != string.Empty)
                {
                    prompt = "Job Application: " + pdfContent + " " + userPrompt;
                }

                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    store = true
                };

                StringContent content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                System.Threading.Tasks.Task<HttpResponseMessage> response = client.PostAsync(uri, content);

                if (response != null)
                {
                    string responseString = response.Result.Content.ReadAsStringAsync().Result;

                    JObject data = (JObject)JsonConvert.DeserializeObject(responseString);
                    string message = data["choices"].First()["message"]["content"].ToString();

                    Console.WriteLine(message);
                }

                Console.Write("> ");
                userPrompt = Console.ReadLine();

            } while (userPrompt != string.Empty);

            Console.Write("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
