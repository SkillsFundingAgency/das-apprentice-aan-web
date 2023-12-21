using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.Aan.SharedUi.Models;

public abstract class MaturityViewModelBase
{
    public bool ShowJoinedDate { get; set; }
    public DateTime JoinedDate { get; set; }
    public bool ShowMaturityTag { get; set; }
    public MemberMaturityStatus MaturityStatus => JoinedDate.Date.AddDays(90) >= DateTime.Today ? MemberMaturityStatus.New : MemberMaturityStatus.Active;
    public string MaturityTagStyle => MaturityStatus == MemberMaturityStatus.New ? "govuk-tag--green" : "govuk-tag--blue";
}