using Newtonsoft.Json;
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
        private static WebInterface instance = null;

        public static WebInterface getInstance(){
            if(instance == null){
                instance =  new WebInterface();
            } 

            return instance;
        }

        private WebInterface()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("");
        } 

        public async Task<T> Get<T>(string path)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(path);

                response.EnsureSuccessStatusCode();
                var respString =  await response.Content.ReadAsStringAsync();
                
                return JsonConvert.DeserializeObject<T>(respString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }
}
