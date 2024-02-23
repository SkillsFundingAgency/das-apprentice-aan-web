namespace SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
public class PersonalDetailsModel(string fullName, string regionName, MemberUserType userType, string personalDetailChangeUrl, string areaOfInterestChangeUrl, string contactDetailChangeUrl, string memberProfileUrl)
{
    public string FullName { get; set; } = fullName;
    public string RegionName { get; set; } = regionName;
    public MemberUserType UserType { get; set; } = userType;
    public string PersonalDetailChangeUrl { get; set; } = personalDetailChangeUrl;
    public string AreaOfInterestChangeUrl { get; set; } = areaOfInterestChangeUrl;
    public string ContactDetailChangeUrl { get; set; } = contactDetailChangeUrl;
    public string MemberProfileUrl { get; set; } = memberProfileUrl;
}