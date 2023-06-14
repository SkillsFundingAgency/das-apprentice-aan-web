namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

public record struct Attendee(Guid MemberId, string UserType, string MemberName);
