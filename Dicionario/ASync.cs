using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dicionario
{
    public static class ASync
    {
        public static async Task<HttpResponseMessage> GetResponseMessage(HttpClient client, string lastPartOfUri)
        {
            return await client.GetAsync(lastPartOfUri);
        }

        public static async Task<string> GetContentFromResponse(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
    }
}
