using System.Text;
using System.Net.Http.Headers;
using TaskListManager.Data.ViewModel;
using TaskListManager.Data.Enums;
using Newtonsoft.Json;

namespace TaskListManager.Business.Factory
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResponse> Get<TResponse>(string baseUrl, string requestUri, string? accessToken, List<CustomHeader> customHeaders) where TResponse : class
        {
            HttpResponseMessage? response = null;

            try
            {
                using var client = _httpClientFactory.CreateClient();
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{requestUri}");
                SetAuthorization(client, baseUrl, accessToken, customHeaders);

                response = await client.SendAsync(request);
                return await ReadResult<TResponse>(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                response?.Dispose();
            }
        }

        public async Task<TResponse> Post<TResponse>(string data, string baseUrl, string requestUri, string? accessToken = null, List<CustomHeader> customHeaders = default) where TResponse : class
        {
            return await HttpRequest<TResponse>(data, baseUrl, requestUri, HttpAction.Post, accessToken, customHeaders);
        }

        public async Task<TResponse> Put<TResponse>(string data, string baseUrl, string requestUri, string? accessToken = null, List<CustomHeader> customHeaders = default) where TResponse : class
        {
            return await HttpRequest<TResponse>(data, baseUrl, requestUri, HttpAction.Put, accessToken, customHeaders);
        }

        public async Task<TResponse> Patch<TResponse>(string data, string baseUrl, string requestUri, string? accessToken = null, List<CustomHeader> customHeaders = default) where TResponse : class
        {
            return await HttpRequest<TResponse>(data, baseUrl, requestUri, HttpAction.Patch, accessToken, customHeaders);
        }

        public async Task<TResponse> Delete<TResponse>(string data, string baseUrl, string requestUri, string? accessToken = null, List<CustomHeader> customHeaders = default) where TResponse : class
        {
            return await HttpRequest<TResponse>(data, baseUrl, requestUri, HttpAction.Delete, accessToken, customHeaders);
        }

        private async Task<TResponse> HttpRequest<TResponse>(string data, string baseUrl, string requestUri, HttpAction httpAction, string? accessToken, List<CustomHeader> customHeaders) where TResponse : class
        {
            HttpResponseMessage? response = null;

            try
            {
                using var client = _httpClientFactory.CreateClient();
                SetAuthorization(client, baseUrl, accessToken, customHeaders);

                var httpContent = new StringContent(data, Encoding.UTF8, "application/json");
                httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                switch (httpAction)
                {
                    case HttpAction.Post:
                        response = await client.PostAsync($"{requestUri}", httpContent);
                        break;
                    case HttpAction.Put:
                        response = await client.PutAsync($"{requestUri}", httpContent);
                        break;
                    case HttpAction.Patch:
                        response = await client.PatchAsync($"{requestUri}", httpContent);
                        break;
                    case HttpAction.Delete:
                        response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, $"{requestUri}") { Content = httpContent });
                        break;
                    default:
                        break;
                }
                var actionStr = httpAction.ToString();

                return await ReadResult<TResponse>(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                response?.Dispose();
            }
        }

        private async Task<TResponse> ReadResult<TResponse>(HttpResponseMessage response) where TResponse : class
        {
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }
            return JsonConvert.DeserializeObject<TResponse>(result);
        }

        /*private async Task SetAuthorization(HttpClient client, string baseUrl, string? accessToken, List<CustomHeader> customHeaders)
        {
            client.BaseAddress = new Uri(baseUrl);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            if (customHeaders.Count() > 0)
            {
                foreach (var customHeader in customHeaders)
                {
                    if (customHeader.Name != "Content-Type")
                        client.DefaultRequestHeaders.Add(customHeader.Name, customHeader.Value);
                    else
                        client.DefaultRequestHeaders.TryAddWithoutValidation(customHeader.Name, customHeader.Value);
                }
            }
        }*/
        private void SetAuthorization(HttpClient client, string baseUrl, string? accessToken, List<CustomHeader> customHeaders = null)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be null or empty");

            client.DefaultRequestHeaders.Clear();

            client.BaseAddress = new Uri(baseUrl);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            if (customHeaders?.Any() == true)
            {
                foreach (var header in customHeaders.Where(h => h != null))
                {
                    try
                    {
                        if (string.Equals(header.Name, "Content-Type", StringComparison.OrdinalIgnoreCase))
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(header.Name, header.Value);
                        }
                        else if (!string.IsNullOrWhiteSpace(header.Name))
                        {
                            client.DefaultRequestHeaders.Remove(header.Name);
                            client.DefaultRequestHeaders.Add(header.Name, header.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
        }
    }

}
