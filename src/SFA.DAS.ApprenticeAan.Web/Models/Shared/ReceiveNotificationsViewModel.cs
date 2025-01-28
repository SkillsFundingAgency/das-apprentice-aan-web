using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApprenticeAan.Web.Models.Shared;

public class ReceiveNotificationsViewModel : ReceiveNotificationsSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

public class ReceiveNotificationsSubmitModel
{
    public bool? ReceiveNotifications { get; set; }
}