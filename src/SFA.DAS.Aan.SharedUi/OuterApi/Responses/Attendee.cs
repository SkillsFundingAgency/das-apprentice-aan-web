namespace SFA.DAS.Aan.SharedUi.OuterApi.Responses;

public record struct Attendee(Guid MemberId, string UserType, string MemberName, DateTime? AddedDate, string MemberProfileLink);
