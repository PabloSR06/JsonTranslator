using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace JsonTranslator
{

    class Program
    {
        private static readonly string key = "<api-key>";
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com";

        private static readonly string location = "northeurope";

        private static readonly string file_location = "<file-location>";

        private static readonly string language_long = "fr-fr";
        private static readonly string language_short = "fr";

        static async Task Main(string[] args)
        {
            // Input and output languages are defined as parameters.
            string route = "/translate?api-version=3.0&from=en&to="+language_short;
            
            var requestBody = File.ReadAllText(file_location + "request.json");
            var jsonRequest = JsonConvert.DeserializeObject<List<Uterance>>(requestBody);

            List<Uterance> jsonResponse = new List<Uterance>();
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                // location required if you're using a multi-service or regional (not global) resource.
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);

                File.WriteAllText(file_location + language_short + "-text.json", JsonConvert.SerializeObject(result));


                var translationList = JsonConvert.DeserializeObject<List<TranslationList>>(result);

                int i = 0;
                foreach (var translationItem in translationList)
                {
                    var translation = translationItem.Translations.FirstOrDefault();
                    jsonResponse.Add(new Uterance
                    {
                        Intent = jsonRequest[i].Intent,
                        Language = language_long,
                        Text = translation.Text,
                        Dataset = jsonRequest[i].Dataset
                    });
                    i++;
                }
            }


            File.WriteAllText(file_location + language_short + "-response.json", JsonConvert.SerializeObject(jsonResponse));
            Console.WriteLine("FIN");
        }
        
    }

     class Uterance
    {
        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("entities")]
        public List<object> Entities { get; set; }

        [JsonProperty("dataset")]
        public String Dataset { get; set; }
    }
    class TranslationList
    {
        [JsonProperty("translations")]
        public List<Translation> Translations { get; set; }
    }
    class Translation
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }

           
}