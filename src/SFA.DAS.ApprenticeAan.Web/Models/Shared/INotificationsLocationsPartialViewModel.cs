using Microsoft.AspNetCore.Mvc.Rendering;

namespace SFA.DAS.ApprenticeAan.Web.Models.Shared
{
    public interface INotificationsLocationsPartialViewModel : INotificationsLocationsPartialSubmitModel, IBackLink
    {
        string Title { get; set; }
        string IntroText { get;set; }

        List<string> SubmittedLocations { get; set; }
        string UnrecognisedLocation { get; set; }
        List<SelectListItem> RadiusOptions { get; }
        int MaxLocations { get; }
        string MaxLocationsString { get; }
    }

    public interface INotificationsLocationsPartialSubmitModel
    {
        string? Location { get; set; }
        int Radius { get; set; }
        string SubmitButton { get; set; }
        bool HasSubmittedLocations { get; set; }
    }

    public static class NotificationsLocationsSubmitButtonOption
    {
        public const string Add = "Add";
        public const string Continue = "Continue";
        public const string Delete = "Delete";
    }
}