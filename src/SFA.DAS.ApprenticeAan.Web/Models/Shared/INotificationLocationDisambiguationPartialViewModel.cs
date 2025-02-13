namespace SFA.DAS.ApprenticeAan.Web.Models.Shared
{
    public interface INotificationLocationDisambiguationPartialViewModel : INotificationLocationDisambiguationPartialSubmitModel, IBackLink
    {
        string BackLink { get; set; }
        string Title { get; set; }
        List<LocationModel>? Locations { get; set; }
    }

    public interface INotificationLocationDisambiguationPartialSubmitModel
    {
        string Location { get; set; }
        int Radius { get; set; }
        string? SelectedLocation { get; set; }
    }
}