
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Application.Services;

public class CalendarService : ICalendarService
{
    private readonly IOuterApiClient _outerApiClient;

    public CalendarService(IOuterApiClient outerApiClient) => _outerApiClient = outerApiClient;

    public async Task<List<Calendar>> GetCalendars()
    {
        var result = await _outerApiClient.GetCalendars();
        return result.OrderBy(x => x.Ordering).ToList();
    }
}