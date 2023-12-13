namespace SFA.DAS.Aan.SharedUi.Models.PublicProfile;
public class MemberProfileMappingModel
{
    public int LinkedinProfileId { get; set; }
    public int JobTitleProfileId { get; set; }
    public int BiographyProfileId { get; set; }
    public List<int> FirstSectionProfileIds { get; set; } = new List<int>();
    public List<int> SecondSectionProfileIds { get; set; } = new List<int>();
    public List<int> AddressProfileIds { get; set; } = new List<int>();
    public int EmployerNameProfileId { get; set; }
}
