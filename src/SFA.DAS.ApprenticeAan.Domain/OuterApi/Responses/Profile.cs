namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses
{
    public class Profile
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int Ordering { get; set; }
    }
}