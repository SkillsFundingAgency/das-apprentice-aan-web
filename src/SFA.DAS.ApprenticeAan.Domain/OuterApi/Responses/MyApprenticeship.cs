namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

public class MyApprenticeship
{
    public long? Uln { get; set; }
    public long? ApprenticeshipId { get; set; }
    public string? EmployerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? TrainingProviderId { get; set; }
    public string? TrainingProviderName { get; set; }

    public TrainingCourse TrainingCourse { get; set; } = null!;
}

public class TrainingCourse
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int Duration { get; set; }
    public string Sector { get; set; }
}
