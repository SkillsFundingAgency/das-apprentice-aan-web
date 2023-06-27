using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface ICalendarService
{
    Task<List<Calendar>> GetCalendars();
}