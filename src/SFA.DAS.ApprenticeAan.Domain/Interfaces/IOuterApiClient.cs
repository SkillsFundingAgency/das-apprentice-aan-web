using System.Threading;
using RestEase;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface IOuterApiClient
{
    [Get("/regions")]
    Task<GetRegionsResult> GetRegions();

    [Get("/calendars")]
    Task<List<Calendar>> GetCalendars();

    [Get("/profiles/{userType}")]
    Task<GetProfilesResult> GetProfilesByUserType([Path("userType")] string userType, CancellationToken? cancellationToken);

    [Get("/apprentices/{apprenticeId}/account")]
    [AllowAnyStatusCode]
    Task<Response<ApprenticeAccount?>> GetApprenticeAccount([Path] Guid apprenticeId);

    [Get("/locations")]
    Task<GetAddressesResult> GetAddresses([Query] string query);

    [Get("/locations/{postcode}")]
    [AllowAnyStatusCode]
    Task<Response<GetCoordinatesResult>> GetCoordinates([Path] string postcode);

    [Get("/apprentices/{apprenticeId}")]
    [AllowAnyStatusCode]
    Task<Response<Apprentice?>> GetApprentice([Path] Guid apprenticeId);

    [Get("calendarEvents")]
    Task<GetCalendarEventsQueryResult> GetCalendarEvents([Header(RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [QueryMap] IDictionary<string, string[]> parameters, CancellationToken cancellationToken);


    [Get("/myapprenticeship/{apprenticeId}")]
    [AllowAnyStatusCode]
    Task<Response<MyApprenticeship>> GetMyApprenticeship([Path] Guid apprenticeId, CancellationToken cancellationToken);

    [Post("/myapprenticeship")]
    Task CreateMyApprenticeship([Body] CreateMyApprenticeshipRequest body, CancellationToken cancellationToken);

    [Post("/apprentices")]
    Task<CreateApprenticeMemberResponse> PostApprenticeMember([Body] CreateApprenticeMemberRequest request);

    [Get("/calendarevents/{calendarEventId}")]
    [AllowAnyStatusCode]
    Task<Response<CalendarEvent>> GetCalendarEventDetails([Path] Guid calendarEventId,
        [Header(RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        CancellationToken cancellationToken);

    [Get("/stagedapprentices")]
    [AllowAnyStatusCode]
    Task<Response<StagedApprentice?>> GetStagedApprentice([Query] string lastName, [Query] string dateOfBirth, [Query] string email, CancellationToken cancellationToken);

    [Put("/calendarevents/{calendarEventId}/attendance")]
    Task PutAttendance([Path] Guid calendarEventId,
                       [Header(RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
                       [Body] SetAttendanceStatusRequest newStatus,
                       CancellationToken cancellationToken);

    [Get("/attendances")]
    Task<GetAttendancesResponse> GetAttendances([Header(RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, string fromDate, string toDate, CancellationToken cancellationToken);

    [Get("/members")]
    Task<GetNetworkDirectoryQueryResult> GetMembers([QueryMap] IDictionary<string, string[]> parameters, CancellationToken cancellationToken);

    [Get("/members/{memberId}/profile")]
    Task<GetMemberProfileResponse> GetMemberProfile([Path] Guid memberId, [Header(RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [Query] bool @public, CancellationToken cancellationToken);
}
