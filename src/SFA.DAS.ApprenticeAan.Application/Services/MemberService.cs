using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;

namespace SFA.DAS.ApprenticeAan.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IOuterApiClient _client;

        public MemberService(IOuterApiClient client)
        {
            _client = client;
        }

        public async Task UpdateMemberDetails(Guid apprenticeId, string firstName, string lastName, CancellationToken cancellationToken)
        {
            UpdateMemberProfileAndPreferencesRequest updateMemberProfileRequest = new()
            {
                PatchMemberRequest = new PatchMemberRequest()
                {
                    FirstName = firstName,
                    LastName = lastName
                }
            };
            await _client.UpdateMemberProfileAndPreferences(apprenticeId, updateMemberProfileRequest, cancellationToken);
        }
    }
}
