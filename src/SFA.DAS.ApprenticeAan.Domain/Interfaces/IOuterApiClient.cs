﻿using RestEase;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface IOuterApiClient
{
    [Get("/regions")]
    Task<GetRegionsResult> GetRegions();

    [Get("/profiles/{userType}")]
    Task<GetProfilesResult> GetProfilesByUserType([Path("userType")] string userType);

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
    Task<GetCalendarEventsQueryResult> GetCalendarEvents([Header(Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [Query] string startDate, [Query] string endDate, CancellationToken cancellationToken);

    [Get("/myapprenticeship/{apprenticeId}")]
    Task<MyApprenticeship> GetMyApprenticeship([Path] Guid apprenticeId);

    [Post("/apprentices")]
    Task<CreateApprenticeMemberResponse> PostApprenticeMember([Body] CreateApprenticeMemberRequest request);
}
