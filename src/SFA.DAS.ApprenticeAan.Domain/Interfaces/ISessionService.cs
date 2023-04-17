namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface ISessionService
{
    void Set<T>(T model);
    T Get<T>();
    void Delete<T>();
    void Clear();
    bool Contains<T>();
}