namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        //public UserType UserType { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime JoinedDate { get; set; }
        public int? RegionId { get; set; }
        public string? OrganisationName { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool? IsRegionalChair { get; set; }
        public string FullName { get; set; } = null!;
    }
}
