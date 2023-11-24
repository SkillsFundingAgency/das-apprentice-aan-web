namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;

public class UpdateMemberProfileAndPreferencesRequest
{
    public PatchMemberRequest? patchMemberRequest { get; set; } = new PatchMemberRequest();
    public UpdateMemberProfileRequest updateMemberProfileRequest { get; set; } = new UpdateMemberProfileRequest();

}

public class PatchMemberRequest
{
    public int RegionId { get; set; }
    public string? OrganisationName { get; set; }
}

public class UpdateMemberProfileRequest
{
    public IEnumerable<UpdateProfileModel> Profiles { get; set; } = Enumerable.Empty<UpdateProfileModel>();
    public IEnumerable<UpdatePreferenceModel> Preferences { get; set; } = Enumerable.Empty<UpdatePreferenceModel>();
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
