using Microsoft.AspNetCore.Mvc.Rendering;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;

namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

public class NotificationsLocationsViewModel : NotificationsLocationsSubmitModel, INotificationsLocationsPartialViewModel
{
    public string BackLink { get; set; } = null!;
    public string Title { get; set; } = "";
    public string IntroText { get; set; } = "";

    public List<string> SubmittedLocations { get; set; } = [];
    public string UnrecognisedLocation { get; set; } = "";
    public string DuplicateLocation { get; set; } = "";
    public int MaxLocations => 5;
    public string MaxLocationsString => "five";
}

public class NotificationsLocationsSubmitModel : INotificationsLocationsPartialSubmitModel
{
    public string? Location { get; set; }
    public int Radius { get; set; } = 5;
    public string SubmitButton { get; set; } = "";
    public bool HasSubmittedLocations { get; set; }

    public List<SelectListItem> RadiusOptions =>
    [
        new SelectListItem("5 miles", "5"),
        new SelectListItem("10 miles", "10"),
        new SelectListItem("20 miles", "20"),
        new SelectListItem("30 miles", "30"),
        new SelectListItem("50 miles", "50"),
        new SelectListItem("100 miles", "100"),
        new SelectListItem("Across England", "0")
    ];
}
