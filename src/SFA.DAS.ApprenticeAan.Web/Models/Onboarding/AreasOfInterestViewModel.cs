namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding
{
    public class AreasOfInterestViewModel : AreasOfInterestSubmitModel, IBackLink
    {
        public string BackLink { get; set; } = null!;
    }

    public class AreasOfInterestSubmitModel
    {
        public List<SelectProfileModel> Events { get; set; } = null!;
        public List<SelectProfileModel> Promotions { get; set; } = null!;
        public IEnumerable<SelectProfileModel> AreasOfInterest => Events.Concat(Promotions);
    }
}