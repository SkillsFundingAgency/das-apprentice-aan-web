using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;
using System.Text;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Validators.Onboarding
{
    [TestFixture]
    public class JoinTheNetworkSubmitModelValidatorTests
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        public void Validates_ReasonForJoiningTheNetwork_NullOrEmpty(string? value, bool isValid)
        {
            var model = new JoinTheNetworkViewModel { ReasonForJoiningTheNetwork = value };
            var sut = new JoinTheNetworkSubmitModelValidator();

            var result = sut.TestValidate(model);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.ReasonForJoiningTheNetwork);
            else
                result.ShouldHaveValidationErrorFor(x => x.ReasonForJoiningTheNetwork)
                    .WithErrorMessage(JoinTheNetworkSubmitModelValidator.ReasonForJoiningTheNetworkEmptyMessage);
        }

        private static string GetParagraph(string str, int ctr)
        {
            StringBuilder sb = new();
            for (int i = 0; i < ctr; i++)
            {
                sb.Append(str);
            }
            return sb.ToString();
        }

        [TestCase(250, true)]
        [TestCase(251, false)]
        public void Validates_ReasonForJoiningTheNetwork_Max(int maxWords, bool isValid)
        {
            var word = "test ";
            var paragraph = GetParagraph(word, maxWords);
            var model = new JoinTheNetworkViewModel { ReasonForJoiningTheNetwork = paragraph };
            var sut = new JoinTheNetworkSubmitModelValidator();

            var result = sut.TestValidate(model);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.ReasonForJoiningTheNetwork);
            else
                result.ShouldHaveValidationErrorFor(c => c.ReasonForJoiningTheNetwork)
                    .WithErrorMessage(JoinTheNetworkSubmitModelValidator.ReasonForJoiningTheNetworkMaxWordsMessage);
        }
    }
}