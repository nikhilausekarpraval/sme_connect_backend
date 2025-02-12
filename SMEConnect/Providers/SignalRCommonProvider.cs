
using SMEConnect.Controllers;
using SMEConnect.Helpers;

namespace SMEConnect.Providers
{
    public class SignalRCommonProvider : ISignalRCommonProvider
    {
        public SignalRCommonProvider() {
        
        }

        public async Task<HttpResponseMessage> PostAsync<T>(HttpClient httpClient, string baseUrl, string url, T data, string token)
        {
            var jsonContent = Helper.GetSerializedData(data);
            Helper.AddAuthorizationHeader(httpClient, token);
            return await httpClient.PostAsync(baseUrl + url, jsonContent);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(HttpClient httpClient, string baseUrl, string url, T data, string token)
        {
            var jsonContent = Helper.GetSerializedData(data);
            Helper.AddAuthorizationHeader(httpClient, token);
            return await httpClient.PutAsync(baseUrl + url, jsonContent);
        }

        public async Task<HttpResponseMessage> GetAsync(HttpClient httpClient, string baseUrl, string url, string token)
        {
            Helper.AddAuthorizationHeader(httpClient, token);
            return await httpClient.GetAsync(baseUrl + url);
        }

        public async Task<HttpResponseMessage> DeleteAsync(HttpClient httpClient, string baseUrl, string url, string token)
        {
            Helper.AddAuthorizationHeader(httpClient, token);
            return await httpClient.DeleteAsync(baseUrl + url);
        }
    }
}
