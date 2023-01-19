using System.Net;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<T> Get<T>(string uri);
        Task<HttpStatusCode> Post<T>(string uri, T model);
    }
}
