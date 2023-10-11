namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

public class GetNotificationResult
{
    public Guid MemberId { get; set; }
    public string TemplateName { get; set; } = null!;
    public DateTime SentTime { get; set; }
    public string? ReferenceId { get; set; }
}
