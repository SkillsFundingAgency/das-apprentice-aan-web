namespace SFA.DAS.ApprenticeAan.Web.Models;

public class OnboardingSessionModel
{
    public ApprenticeDetailsModel ApprenticeDetails { get; set; } = new ApprenticeDetailsModel();
    public bool HasAcceptedTermsAndConditions { get; set; } = false;
    public bool? HasEmployersApproval { get; set; }
    public List<ProfileModel> ProfileData { get; set; } = new List<ProfileModel>();
    public int? RegionId { get; set; }
    public bool IsValid =>
        ApprenticeDetails.ApprenticeId.GetValueOrDefault() != Guid.Empty &&
        HasAcceptedTermsAndConditions && HasEmployersApproval.GetValueOrDefault() &&
        ProfileData.Count > 0;

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

public static class OnboardingSessionModelExtensionMethods
{
    public static string GetProfileModelValue(this List<ProfileModel> profileItems, int id)
    {
        return profileItems.FirstOrDefault(e => e.Id == id)?.Value!;
    }

    public static IEnumerable<ProfileModel> UpdateProfileModelValue(this List<ProfileModel> profileItems, int id, string? value)
    {
        foreach (var profileItem in profileItems)
        {
            if (profileItem.Id == id)
                profileItem.Value = value != null ? value! : null!;

            yield return profileItem;
        }
    }
}