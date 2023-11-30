namespace SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
public class PersonalDetailsModel
{
    public PersonalDetailsModel(string fullName, string regionName, MemberUserType userType, string personalDetailChangeUrl, string areaOfInterestChangeUrl)
    {
        FullName = fullName;
        RegionName = regionName;
        UserType = userType;
        PersonalDetailChangeUrl = personalDetailChangeUrl;
        AreaOfInterestChangeUrl = areaOfInterestChangeUrl;
    }

    public string FullName { get; set; }
    public string RegionName { get; set; }
    public MemberUserType UserType { get; set; }
    public string PersonalDetailChangeUrl { get; set; }
    public string AreaOfInterestChangeUrl { get; set; }
}