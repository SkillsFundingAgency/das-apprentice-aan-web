namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class SelectedFilter
{
    public string FieldName { get; set; } = null!;
    public int FieldOrder { get; set; }
    public List<EventFilterItem> Filters { get; set; } = new List<EventFilterItem>();
}