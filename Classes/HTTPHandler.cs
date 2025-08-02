using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace ResumeParser.Classes
{
    internal class HTTPHandler
    {
        private string message;
        private string conversation;
        private bool requestError;
        public bool RequestError
        {
            get { return requestError; }
            set { requestError = value; }
        }
        public HTTPHandler()
        {
            message = string.Empty;
            conversation = string.Empty;
            RequestError = false;
        }

        public HttpClient CreateClient(Uri baseURI, AuthenticationHeaderValue headerValue)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseURI;
            httpClient.DefaultRequestHeaders.Authorization = headerValue;

            return httpClient;
        }

        public string PostPrompt(AIHandler aiHandler, HttpClient client, string userPrompt, string pdfContent)
        {
            if (userPrompt != string.Empty)
            {
                aiHandler.Prompt = "Job Application: " + pdfContent + " Previous reply from chatgpt model: " + conversation + " New prompt: " + userPrompt;
            }
            else
            {
                message = string.Empty;
                aiHandler.Prompt += pdfContent;

                conversation += " " + aiHandler.Prompt;
            }

            var requestBody = new
            {
                model = aiHandler.Model,
                messages = new[]
                {
                    new { role = "user", content = "Reply in english if not instructed otherwise. " + aiHandler.Prompt }
                },
                store = true
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            System.Threading.Tasks.Task<HttpResponseMessage> response = client.PostAsync(aiHandler.Uri, content);

            if (response != null)
            {
                string responseString = response.Result.Content.ReadAsStringAsync().Result;

                JObject data = (JObject)JsonConvert.DeserializeObject(responseString);
                try
                {
                    message = data["choices"].First()["message"]["content"].ToString();
                    conversation += " " + message;
                    RequestError = false;
                }
                catch (Exception ex)
                {
                    message = data["error"]["message"].ToString();
                    RequestError = true;
                }
            }

            return message;

        }
    }
}
