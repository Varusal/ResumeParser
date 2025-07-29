using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ResumeParser.Classes
{
    internal class HTTPHandler
    {
        
        public HTTPHandler()
        {

        }

        public HttpClient CreateClient(Uri baseURI, AuthenticationHeaderValue headerValue)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseURI;
            httpClient.DefaultRequestHeaders.Authorization = headerValue;

            return httpClient;
        }

        public string PostPrompt(AIHandler aiHandler,HttpClient client, string userPrompt, string pdfContent)
        {
            string message = string.Empty;
            if (userPrompt != string.Empty)
            {
                aiHandler.Prompt = "Job Application: " + pdfContent + " " + userPrompt;
            }
            else
            {
                aiHandler.Prompt += pdfContent;
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
                message = data["choices"].First()["message"]["content"].ToString();

                //Console.WriteLine(message);
            }
            return message;
        }
    }
}
