using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding
{
    public class RegionsViewModel : IBackLink
    {
        public List<Region> Regions { get; set; }
        public string BackLink { get; set; } = null!;
    }
}