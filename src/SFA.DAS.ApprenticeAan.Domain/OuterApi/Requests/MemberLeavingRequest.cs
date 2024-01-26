namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;

public class MemberLeavingRequest
{
    public List<int> LeavingReasons { get; set; } = new();
}