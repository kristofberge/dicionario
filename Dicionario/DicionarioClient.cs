using Dicionario.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dicionario
{
    public class DicionarioClient
    {
        private string _baseAddress;
        private string _acceptHeader;
        private HttpClient _client;
        
        public DicionarioClient()
        {
            _baseAddress = "http://teste.way2.com.br/dic/";
            _acceptHeader = "text/plain";
        }

        public virtual string GetWordAtPosition(int position)
        {
            HttpResponseMessage response = GetResponseFomDicionario(position);

            if (response.IsSuccessStatusCode)
                return ASync.GetContentFromResponse(response).Result;

            throw new NoWordAtPositionException();
        }

        protected virtual HttpResponseMessage GetResponseFomDicionario(int position)
        {
            if(_client == null)
                InitialiseClient();

            string lastPartOfUri = "api/words/" + position;

            return ASync.GetResponseMessage(_client, lastPartOfUri).Result;
            
        }

        private void InitialiseClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_acceptHeader));
        }
    }
}
