namespace SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;

public class SubmitAreaOfInterestModel : INetworkHubLink
{
    public List<SelectProfileViewModel> Events { get; set; } = null!;
    public List<SelectProfileViewModel> Promotions { get; set; } = null!;
    public IEnumerable<SelectProfileViewModel> AreasOfInterest => Events.Concat(Promotions);
    public string? NetworkHubLink { get; set; }
}
