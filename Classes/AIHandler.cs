namespace ResumeParser.Classes
{
    internal class AIHandler
    {
        private string prompt;
        public string Prompt
        {
            get { return prompt; }
            set { prompt = value; }
        }

        private string model;
        public string Model
        {
            get { return model; }
            set {  model = value; }
        }

        private string uri;
        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        private string apiKey;
        public string ApiKey
        {
            get { return apiKey; }
            set { apiKey = value; }
        }

        private Uri baseuri;
        public Uri BaseURI
        {
            get { return baseuri; }
            set { baseuri = value; }
        }

        public AIHandler()
        {
            prompt = string.Empty;
            model = string.Empty;
            uri = string.Empty;
            apiKey = string.Empty;
            baseuri = new Uri("about:blank");
        }
    }
}
