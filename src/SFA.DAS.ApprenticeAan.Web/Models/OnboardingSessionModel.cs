namespace SFA.DAS.ApprenticeAan.Web.Models;

public class OnboardingSessionModel
{
    public ApprenticeDetailsModel ApprenticeDetails { get; set; } = new ApprenticeDetailsModel();
    public bool HasAcceptedTermsAndConditions { get; set; } = false;
    public bool HasEmployersApproval { get; set; } = false;
    public List<ProfileModel> ProfileData { get; set; } = new List<ProfileModel>();
    public int? RegionId { get; set; }

    public bool IsValid => ApprenticeDetails.ApprenticeId != null && ApprenticeDetails.ApprenticeId != Guid.Empty && HasAcceptedTermsAndConditions && HasEmployersApproval;
}

public class ApprenticeDetailsModel
{
    public Guid? ApprenticeId { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? ReasonForJoiningTheNetwork { get; set; }  //maps to information field in apprentice table
}

public class ProfileModel
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
    public string Category { get; set; } = null!;
}
