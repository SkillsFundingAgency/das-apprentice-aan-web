namespace SFA.DAS.ApprenticeAan.Web.Models;

public class OnboardingSessionModel
{
    public bool HasSeenPreview { get; set; }
    public ApprenticeDetailsModel ApprenticeDetails { get; set; } = new ApprenticeDetailsModel();
    public bool HasAcceptedTerms { get; set; } = false;
    public List<ProfileModel> ProfileData { get; set; } = new List<ProfileModel>();
    public int? RegionId { get; set; }
    public string? RegionName { get; set; }


    public string? GetProfileValue(int id) => ProfileData.Single(p => p.Id == id)?.Value;

    public void SetProfileValue(int id, string value) => ProfileData.Single(p => p.Id == id).Value = value;
}

public class ApprenticeDetailsModel
{
    public Guid? ApprenticeId { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? ReasonForJoiningTheNetwork { get; set; }  //maps to information field in apprentice table
}
