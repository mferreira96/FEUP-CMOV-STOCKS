using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace my_stocks.services
{
    class WebInterface
    {
        private HttpClient _client;
        private static WebInterface instance = null;

        public static WebInterface GetInstance(){
            if(instance == null){
                instance =  new WebInterface();
            } 

            return instance;
        }

        private WebInterface()
        {
            _client = new HttpClient();
            String url;
            try
            {
                url = App.Current.Properties["url"].ToString();
            }catch(Exception e)
            {
                url = "http://localhost:8080";
                Console.WriteLine(e.Message);
            }
            
            _client.BaseAddress = new Uri(url);
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
