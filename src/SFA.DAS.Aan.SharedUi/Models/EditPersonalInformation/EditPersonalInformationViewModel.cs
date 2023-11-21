namespace SFA.DAS.Aan.SharedUi.Models;
public class EditPersonalInformationViewModel : SubmitPersonalDetailCommand
{
    public string YourAmbassadorProfileUrl { get; set; } = null!;
    public List<RegionViewModel> Regions { get; set; } = new();
}