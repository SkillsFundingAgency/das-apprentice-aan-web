namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;

public class CreateNotificationRequest(Guid memberId, int notificationTemplateId)
{
    public Guid MemberId { get; set; } = memberId;
    public int NotificationTemplateId { get; set; } = notificationTemplateId;
}
