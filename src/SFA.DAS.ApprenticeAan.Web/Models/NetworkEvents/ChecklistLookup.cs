namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
public class ChecklistLookup
{
    public string Name { get; }
    public string Value { get; }
    public bool Checked { get; set; }
    public ChecklistLookup(string name, string value, bool check = false)
    {
        Name = name;
        Value = value;
        Checked = check;
    }
}