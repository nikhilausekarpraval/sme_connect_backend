using SMEConnect.Helpers;

namespace SMEConnect.Controllers
{
    public interface ISignalRCommonProvider
    {
        public Task<HttpResponseMessage> PostAsync<T>(HttpClient httpClient, string baseUrl, string url, T data, string token);


        public Task<HttpResponseMessage> PutAsync<T>(HttpClient httpClient, string baseUrl, string url, T data, string token);


        public Task<HttpResponseMessage> GetAsync(HttpClient httpClient, string baseUrl, string url, string token);


        public Task<HttpResponseMessage> DeleteAsync(HttpClient httpClient, string baseUrl, string url, string token);

    }
}
