using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace SFA.DAS.ApprenticeAan.Infrastructure.ApiClients
{
    [ExcludeFromCodeCoverage]
    public class ApiClient : IApiClient
    {
        protected const string ContentType = "application/json";
        protected readonly HttpClient HttpClient;
        protected readonly ILogger<ApiClient> Logger;

        public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
        {
            HttpClient = httpClient;
            Logger = logger;
        }

        /// <summary>
        /// HTTP GET to the specified URI
        /// </summary>
        /// <typeparam name="T">The type of the object to read.</typeparam>
        /// <param name="uri">The URI to the end point you wish to interact with.</param>
        /// <returns>A Task yielding the result (of type T).</returns>
        /// <exception cref="HttpRequestException">Thrown if something unexpected occurred when sending the request.</exception>
        public async Task<T> Get<T>(string uri)
        {
            try
            {
                using var response = await HttpClient.GetAsync(new Uri(uri, UriKind.Relative));
                await LogErrorIfUnsuccessfulResponse(response);
                return (response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<T>() : (T?)default)!;
            }
            catch (HttpRequestException ex)
            {
                Logger.LogError(ex, "Error when processing request: {HttpMethod.Get} - {uri}", HttpMethod.Get, uri);
                throw;
            }
        }

        /// <summary>
        /// HTTP POST to the specified URI
        /// </summary>
        /// <param name="uri">The URI to the end point you wish to interact with.</param>
        /// <returns>The HttpStatusCode, which is the responsibility of the caller to handle.</returns>
        /// <exception cref="HttpRequestException">Thrown if something unexpected occurred when sending the request.</exception>
        public async Task<HttpStatusCode> Post<T>(string uri, T model)
        {
            var serializeObject = JsonConvert.SerializeObject(model);

            try
            {
                using var response = await HttpClient.PostAsync(new Uri(uri, UriKind.Relative),
                    new StringContent(serializeObject, Encoding.UTF8, ContentType));
                await LogErrorIfUnsuccessfulResponse(response);
                return response.StatusCode;
            }
            catch (HttpRequestException ex)
            {
                Logger.LogError(ex, "Error when processing request: {HttpMethod.Post} - {uri}", HttpMethod.Post, uri);
                throw;
            }
        }

        private async Task LogErrorIfUnsuccessfulResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;
            if (response?.RequestMessage != null)
            {
                var callingMethod = new System.Diagnostics.StackFrame(1).GetMethod()?.Name;

                var httpMethod = response.RequestMessage.Method.ToString();
                var statusCode = (int)response.StatusCode;
                var reasonPhrase = response.ReasonPhrase;
                var requestUri = response.RequestMessage.RequestUri;

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiErrorMessage = responseContent;

                Logger.LogError("Method: {callingMethod} || HTTP {statusCode} {reasonPhrase} || {httpMethod}: {requestUri} || Message: {apiErrorMessage}", callingMethod, statusCode, reasonPhrase, httpMethod, requestUri, apiErrorMessage);
            }
        }
    }
}