using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;

public class CreateMyApprenticeshipRequest
{
    public Guid ApprenticeId { get; set; }
    public long? Uln { get; set; }
    public long? ApprenticeshipId { get; set; }
    public string? EmployerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? TrainingProviderId { get; set; }
    public string? TrainingProviderName { get; set; }
    public string? TrainingCode { get; set; }
    public string? StandardUId { get; set; }

    public static implicit operator CreateMyApprenticeshipRequest(StagedApprentice source) =>
        new()
        {
            Uln = source.Uln,
            ApprenticeshipId = source.ApprenticeshipId,
            EmployerName = source.EmployerName,
            StartDate = source.StartDate,
            EndDate = source.EndDate,
            TrainingProviderId = source.TrainingProviderId,
            TrainingProviderName = source.TrainingProviderName,
            TrainingCode = source.TrainingCode,
            StandardUId = source.StandardUId
        };
}
