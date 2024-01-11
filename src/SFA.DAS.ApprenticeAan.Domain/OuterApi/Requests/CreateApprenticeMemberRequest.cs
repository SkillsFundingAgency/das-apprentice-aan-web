namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;

public class CreateApprenticeMemberRequest
{
    public Guid ApprenticeId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
    public int RegionId { get; set; }
    public string OrganisationName { get; set; } = null!;
    public List<ProfileValue> ProfileValues { get; set; } = new();

    public record class ProfileValue(int Id, string Value);
}