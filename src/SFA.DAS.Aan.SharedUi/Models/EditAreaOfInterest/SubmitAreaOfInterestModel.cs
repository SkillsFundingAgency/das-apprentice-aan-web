namespace SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;

public class SubmitAreaOfInterestModel
{
    public List<SelectProfileViewModel> FirstSectionInterests { get; set; } = null!;
    public List<SelectProfileViewModel> SecondSectionInterests { get; set; } = null!;
    public IEnumerable<SelectProfileViewModel> AreasOfInterest => FirstSectionInterests.Concat(SecondSectionInterests);
}
