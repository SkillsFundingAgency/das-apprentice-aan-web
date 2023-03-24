using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
public class RegionsViewModel : RegionsSubmitModel, IBackLink
{
    public List<Region> Regions { get; set; } = null!;
    public string BackLink { get; set; } = null!;
}

public class RegionsSubmitModel
{
    public int? SelectedRegionId { get; set; }
}