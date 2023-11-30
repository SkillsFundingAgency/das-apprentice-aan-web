namespace SFA.DAS.Aan.SharedUi.Models;
public class EditPersonalInformationViewModel : SubmitPersonalDetailModel
{
    public string YourAmbassadorProfileUrl { get; set; } = null!;
    public List<RegionViewModel> Regions { get; set; } = new();
}