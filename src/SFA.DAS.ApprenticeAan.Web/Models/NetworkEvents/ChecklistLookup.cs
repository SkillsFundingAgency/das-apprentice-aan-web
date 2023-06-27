namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
public class ChecklistLookup
{
    public string Name { get; }
    public string Value { get; }
    public ChecklistLookup(string name, string value)
    {
        Name = name;
        Value = value;
    }
}
