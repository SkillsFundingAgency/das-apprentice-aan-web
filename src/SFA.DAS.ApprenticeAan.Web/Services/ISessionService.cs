namespace SFA.DAS.ApprenticeAan.Web.Services
{
    public interface ISessionService
    {
        void Set(string value, string key);
        void Set<T>(T model);
        string Get(string key);
        T Get<T>();
        void Delete(string key);
        void Delete<T>(T model);
        void Clear();
    }
}