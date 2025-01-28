namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

public class SelectNotificationsViewModel : SelectNotificationsSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

public class SelectNotificationsSubmitModel : IEventTypeViewModel
{
    public List<EventTypeModel> EventTypes { get; set; } = new();
}