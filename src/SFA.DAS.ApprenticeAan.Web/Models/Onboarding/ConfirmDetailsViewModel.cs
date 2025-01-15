namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding
{
    public class ConfirmDetailsViewModel : IBackLink
    {
        public string BackLink { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ApprenticeshipSector { get; set; }
        public string? ApprenticeshipProgram { get; set; }
        public int? ApprenticeshipLevel { get; set; }
    }
}