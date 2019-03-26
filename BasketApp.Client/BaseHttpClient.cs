using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BasketApp.Client
{
    public class BaseHttpClient
    {
        protected readonly HttpClient _httpClient;
        protected string _sessionCookie;
        private string _baseAddress = "http://localhost:5001/";

        public BaseHttpClient()
        {
            var handler = new HttpClientHandler { UseCookies = false };
            _httpClient = new HttpClient(handler) { BaseAddress = new Uri(_baseAddress) };
        }

        public async Task<TResponse> GetAsync<TResponse>(string relativeUri)
        {
            var request = new RequestBuilder(_sessionCookie)
                .WithUrl($"{_baseAddress}{relativeUri}")
                .Build();

            using (var response = await _httpClient.SendAsync(request))
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
        }

        public async Task PostAsync(string relativeUri, object body)
        {
            var request = new RequestBuilder(_sessionCookie)
                .WithMethod(HttpMethod.Put)
                .WithUrl($"{_baseAddress}{relativeUri}")
                .WithContent(body)
                .Build();

            await _httpClient.SendAsync(request);
        }

        public async Task PutAsync(string relativeUri, object body)
        {
            var request = new RequestBuilder(_sessionCookie)
                .WithMethod(HttpMethod.Put)
                .WithUrl($"{_baseAddress}{relativeUri}")
                .WithContent(body)
                .Build();

            await _httpClient.SendAsync(request);
        }

        public async Task DeleteAsync(string relativeUri)
        {
            var request = new RequestBuilder(_sessionCookie)
                .WithMethod(HttpMethod.Delete)
                .WithUrl($"{_baseAddress}{relativeUri}")
                .Build();

            await _httpClient.SendAsync(request);
        }

    }
}