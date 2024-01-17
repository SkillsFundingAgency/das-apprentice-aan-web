namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface ISessionService
{
    void Set(string key, string value);
    void Set<T>(T model);
    T Get<T>();
    string? Get(string key);
    void Delete<T>();
    void Clear();
    bool Contains<T>();
}