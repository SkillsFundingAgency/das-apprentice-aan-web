using Moq;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services
{
    public class MemberServiceTests
    {
        [Test]
        [MoqAutoData]
        public async Task UpdateMemberDetails_InvokesApiClient(
            Mock<IOuterApiClient> clientMock,
            Guid apprenticeId,
            string firstName,
            string lastName,
            CancellationToken cancellationToken
        )
        {
            var updateMemberProfileRequest = new UpdateMemberProfileAndPreferencesRequest
            {
                PatchMemberRequest = new PatchMemberRequest
                {
                    FirstName = firstName,
                    LastName = lastName
                }
            };

            clientMock.Setup(a => a.UpdateMemberProfileAndPreferences(
                apprenticeId,
                It.Is<UpdateMemberProfileAndPreferencesRequest>(request =>
                    request.PatchMemberRequest.FirstName == updateMemberProfileRequest.PatchMemberRequest.FirstName &&
                    request.PatchMemberRequest.LastName == updateMemberProfileRequest.PatchMemberRequest.LastName
                ),
                cancellationToken)
            ).Verifiable();

            MemberService sut = new MemberService(clientMock.Object);

            await sut.UpdateMemberDetails(apprenticeId, firstName, lastName, cancellationToken);

            clientMock.Verify();
        }
    }
}
