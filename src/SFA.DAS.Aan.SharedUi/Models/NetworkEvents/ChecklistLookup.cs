namespace SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
public class ChecklistLookup(string name, string value, bool isChecked = false)
{
    public string Name { get; } = name;
    public string Value { get; } = value;
    public string Checked { get; set; } = isChecked ? "checked" : string.Empty;
}
