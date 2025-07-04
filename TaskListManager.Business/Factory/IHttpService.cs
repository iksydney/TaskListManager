using TaskListManager.Data.ViewModel;

namespace TaskListManager.Business.Factory
{
    public interface IHttpService
    {
        Task<TResponse> Delete<TResponse>(string data, string baseUrl, string requestUri, string? accessToken = null, List<CustomHeader> customHeaders = null) where TResponse : class;
        Task<TResponse> Get<TResponse>(string baseUrl, string requestUri, string? accessToken, List<CustomHeader> customHeaders) where TResponse : class;
        Task<TResponse> Patch<TResponse>(string data, string baseUrl, string requestUri, string? accessToken = null, List<CustomHeader> customHeaders = null) where TResponse : class;
        Task<TResponse> Post<TResponse>(string data, string baseUrl, string requestUri, string? accessToken = null, List<CustomHeader> customHeaders = null) where TResponse : class;
        Task<TResponse> Put<TResponse>(string data, string baseUrl, string requestUri, string? accessToken = null, List<CustomHeader> customHeaders = null) where TResponse : class;
    }
}
