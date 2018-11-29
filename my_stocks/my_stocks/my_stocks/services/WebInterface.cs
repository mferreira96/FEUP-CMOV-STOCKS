using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace my_stocks.services
{
    class WebInterface
    {
        private HttpClient _client;
        private const string key = "ec91612d3e05b8c61018e85a1d1edbf9";
        private const String base_history = "https://marketdata.websol.barchart.com/getHistory.json?apikey=";
        private const String base_quote = "https://marketdata.websol.barchart.com/getQuote.json?apikey=";

        public WebInterface()
        {
            _client = new HttpClient();
        } 

        public async Task MakeGetRequest(string resource, string type)
        {
            try
            {
                string concatenatedUrl;

                if (type.Equals("Quote"))
                {
                    concatenatedUrl = base_quote + key;
                    _client.BaseAddress = new Uri(concatenatedUrl);
                }
                else
                {
                    concatenatedUrl = base_quote + key;
                    _client.BaseAddress = new Uri(concatenatedUrl);
                }

                var request = new HttpRequestMessage()
                {    
                    RequestUri = new Uri(_client.BaseAddress, resource),
                    Method = HttpMethod.Get,
                };

                var response = await _client.SendAsync(request);

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    var result = response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
