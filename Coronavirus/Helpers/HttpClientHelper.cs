using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Coronavirus.Helpers
{
    public class HttpClientHelper : IHttpClientHelper
    {       
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> GetAsync(string uri, IDictionary<string, string> headers = null)
        {
            string result = null;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };
            foreach (KeyValuePair<string, string> header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }            

            using (var response = await client.SendAsync(request))
            {                
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            return result;
        }
    }
}