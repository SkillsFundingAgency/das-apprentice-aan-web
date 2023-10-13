using SFA.DAS.Aan.SharedUi.Models;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses
{
    public class GetProfilesResult
    {
        public List<Profile> Profiles { get; set; } = new();
    }
}
