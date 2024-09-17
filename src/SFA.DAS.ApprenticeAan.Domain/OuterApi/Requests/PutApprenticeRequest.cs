namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;

public class PutApprenticeRequest
{
    public required string Email { get; set; }
    public required string GovUkIdentifier { get; set; }
}