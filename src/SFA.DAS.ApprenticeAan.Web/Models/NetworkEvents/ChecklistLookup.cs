namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
public class ChecklistLookup
{
    public string Name { get; }
    public string Value { get; }
    public bool Checked { get; set; } = false;
    public ChecklistLookup(string name, string value)
    {
        Name = name;
        Value = value;
    }
}