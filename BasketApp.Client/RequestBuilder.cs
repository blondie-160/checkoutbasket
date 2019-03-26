using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace BasketApp.Client
{
    public class RequestBuilder
    {
        private HttpRequestMessage _model;

        public RequestBuilder(string sessionCookie)
        {
            _model = new HttpRequestMessage();
            if (!string.IsNullOrEmpty(sessionCookie))
                _model.Headers.Add("Cookie", $"Session={sessionCookie}");

        }

        public RequestBuilder WithUrl(string relativeUrl)
        {
            _model.RequestUri = new Uri(relativeUrl);
            return this;
        }

        public RequestBuilder WithMethod(HttpMethod method)
        {
            _model.Method = method;
            return this;
        }

        public RequestBuilder WithContent(object body)
        {
            var json = JsonConvert.SerializeObject(body);
            _model.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return this;
        }

        public HttpRequestMessage Build()
        {
            //_model.Headers.Add("Content-Type", "application/json");
            return _model;
        }
    }
}
