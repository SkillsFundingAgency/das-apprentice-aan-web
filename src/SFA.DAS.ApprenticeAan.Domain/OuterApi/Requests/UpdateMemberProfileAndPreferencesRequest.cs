namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;

public class UpdateMemberProfileAndPreferencesRequest
{
    public PatchMemberRequest PatchMemberRequest { get; set; } = new PatchMemberRequest();
    public UpdateMemberProfileRequest UpdateMemberProfileRequest { get; set; } = new UpdateMemberProfileRequest();

}

public class PatchMemberRequest
{
    public int RegionId { get; set; }
    public string? OrganisationName { get; set; }
}

public class UpdateMemberProfileRequest
{
    public IEnumerable<UpdateProfileModel> MemberProfiles { get; set; } = Enumerable.Empty<UpdateProfileModel>();
    public IEnumerable<UpdatePreferenceModel> MemberPreferences { get; set; } = Enumerable.Empty<UpdatePreferenceModel>();
}
public class UpdateProfileModel
{
    public int MemberProfileId { get; set; }
    public string? Value { get; set; }
}
public class UpdatePreferenceModel
{
    public int PreferenceId { get; set; }
    public bool Value { get; set; }
}
