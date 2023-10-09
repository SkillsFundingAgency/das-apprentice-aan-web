using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class MemberProfileMappingModelTest
{
    [Test, RecursiveMoqAutoData]
    public void MemberProfileMappingModel_ReturningExpectedValueFromParameters(int linkedinProfileId, int jobTitleProfileId, int biographyProfileId, List<int> firstSectionProfileIds, List<int> secondSectionProfileIds, List<int> addressProfileIds, int employerNameProfileId, bool isLoggedInUserMemberProfile)
    {
        //Act
        var sut = new MemberProfileMappingModel(linkedinProfileId, jobTitleProfileId, biographyProfileId, firstSectionProfileIds, secondSectionProfileIds, addressProfileIds, employerNameProfileId, isLoggedInUserMemberProfile);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.LinkedinProfileId, Is.EqualTo(linkedinProfileId));
            Assert.That(sut.JobTitleProfileId, Is.EqualTo(jobTitleProfileId));
            Assert.That(sut.BiographyProfileId, Is.EqualTo(biographyProfileId));
            Assert.That(sut.FirstSectionProfileIds, Is.EqualTo(firstSectionProfileIds));
            Assert.That(sut.SecondSectionProfileIds, Is.EqualTo(secondSectionProfileIds));
            Assert.That(sut.AddressProfileIds, Is.EqualTo(addressProfileIds));
            Assert.That(sut.EmployerNameProfileId, Is.EqualTo(employerNameProfileId));
            Assert.That(sut.IsLoggedInUserMemberProfile, Is.EqualTo(isLoggedInUserMemberProfile));
        });
    }
}