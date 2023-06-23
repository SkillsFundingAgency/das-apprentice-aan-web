namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class SelectedFilter
{
    public string FieldName { get; set; } = null!;
    public int FieldOrder { get; set; }
    public List<FilterItem> Filters { get; set; } = new List<FilterItem>();
}